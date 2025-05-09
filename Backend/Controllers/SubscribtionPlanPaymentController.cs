using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Freelancing.Models;
using static Freelancing.Models.Stripe;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Stripe;
using System.Collections.Generic;
using Freelancing.Helpers;
using Stripe.Checkout;
using CloudinaryDotNet;
using Freelancing.DTOs;
using Stripe.Issuing;
using System.Numerics;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Freelancing.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace Freelancing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribtionPlanPaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly StripeSettings _stripeSettings;
        private readonly ILogger<SubscribtionPlanPaymentController> _logger;
        private readonly IHubContext<NotificationHub> _NotifhubContext;

       

        public SubscribtionPlanPaymentController(ApplicationDbContext context, IHubContext<NotificationHub> notifhubContext,  IOptions<StripeSettings> stripeSettings, ILogger<SubscribtionPlanPaymentController> logger)
        {
            _context = context;
            _stripeSettings = stripeSettings.Value;
            _logger = logger;
             _NotifhubContext = notifhubContext;
        }

        [HttpGet("create-checkout-session")]
        public async Task<ActionResult> CreateCheckoutSession(int planId, string redirectionurl)
        {
            try
            {
               // var userId = "4be59d6d-28bd-4a39-93eb-33f304792d84";
                 var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Unauthorized("User not authenticated.");

                // Check if user is a Freelancer or Client
                var freelancer = await _context.freelancers.FirstOrDefaultAsync(f => f.Id == userId);
                var client = await _context.clients.FirstOrDefaultAsync(c => c.Id == userId);

                if (freelancer == null && client == null)
                {
                    return BadRequest(new { Message = "User must be a freelancer or client to subscribe." });
                }

                // Get the plan
                var plan = SubscriptionPlansHelper.SubscriptionPlans.FirstOrDefault(p => p.Id == planId);
                if (plan == null)
                {
                    return BadRequest(new { Message = "Invalid subscription plan." });
                }

                // If it's a free plan, activate it immediately for both freelancers and clients
                if (plan.Price == 0)
                {
                    

                    if (freelancer != null  )
                    {
                        freelancer.subscriptionPlanId = plan.Id;
                        freelancer.RemainingNumberOfBids = plan.TotalNumber;

                    }
                    else if (client != null)
                    {
                        client.RemainingNumberOfProjects = plan.TotalNumber;
                        client.subscriptionPlanId = plan.Id;
                    }

                    await _context.SaveChangesAsync();
                    return Ok(new { Message = "Free plan activated successfully." });
                }

                // var url = $"{Request.Scheme}://{Request.Host}/Stripe/create-checkout-session?Amount={plan.Price}";

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var SuccessUrl = $"{baseUrl}/api/SubscribtionPlanPayment/payment-success?sessionId={{CHECKOUT_SESSION_ID}}&planid={planId}&";


                //var SuccessUrl = $"{baseUrl}/api/SubscribtionPlanPayment/Success?session_id={{CHECKOUT_SESSION_ID}}&proposalId={planId}&";


                //var paymentsucessUrl = Url.ActionLink("paymentsucess", "SubscribtionPlanPayment", new
                //{
                //    sessionId = "{CHECKOUT_SESSION_ID}",
                //    planid = plan.Id

                //});



                var url = Url.ActionLink("CreateCheckoutSession", "Stripe", new
                {
                    Amount = plan.Price,
                    redirectionurl = SuccessUrl,
                });

            return Redirect(url);

            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Stripe error");
                return BadRequest(new { Message = ex.StripeError.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error");
                return StatusCode(500, new { error = "An error occurred while creating the payment session." });
            }

        }





        [HttpGet("PaySubscriptionFromBalance")]
        public async Task<IActionResult> PaySubscriptionFromBalance(int planId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userId = "4be59d6d-28bd-4a39-93eb-33f304792d84";



            if (userId == null)
                return Unauthorized(new { Message = "User not authenticated." });

            var plan = SubscriptionPlansHelper.SubscriptionPlans.FirstOrDefault(p => p.Id == planId);
            if (plan == null)
                return BadRequest(new { Message = "Subscription plan not found." });

            var freelancer = await _context.freelancers.FirstOrDefaultAsync(f => f.Id == userId);
            var client = await _context.clients.FirstOrDefaultAsync(c => c.Id == userId);

            if (freelancer == null && client == null)
                return BadRequest(new { Message = "User must be a freelancer or client." });

            decimal userBalance = freelancer?.Balance ?? client?.Balance ?? 0;

            if (userBalance < plan.Price)
                return BadRequest(new { Message = "Not enough balance." });

            // Deduct balance
            if (freelancer != null)
            {
                freelancer.Balance -= plan.Price;
                freelancer.subscriptionPlanId = plan.Id;
                freelancer.RemainingNumberOfBids = plan.TotalNumber;
            }
            else if (client != null)
            {
                client.Balance -= plan.Price;
                client.subscriptionPlanId = plan.Id;
                client.RemainingNumberOfProjects = plan.TotalNumber;
            }

            // Generate unique TransactionId
            var transactionId = Guid.NewGuid().ToString();

            // Record payment
            var payment = new SubscriptionPayment
            {
                Amount = plan.Price,
                 Date = DateTime.UtcNow,
                PaymentMethod = PaymentMethod.Balance,
                TransactionId = transactionId, // Use same transactionId
                 IsDeleted = false
            };

            _context.SubscriptionPayments.Add(payment);
            await _context.SaveChangesAsync();

            // Link payment to user and plan
            var userPlan = new UserSubscriptionPlanPayment
            {
                UserId = userId,
                SubscriptionPlanId = plan.Id,
                SubscriptionPaymentId = payment.Id,
                isDeleted = false
            };

            _context.UserSubscriptionPlanPayments.Add(userPlan);
            await _context.SaveChangesAsync();

            // Redirect to the success page with the same transactionId
            //  var url = $"/payments/subscription-success?transactionId={transactionId}";
            //  var url = Url.Action("paymentsucess", "SubscribtionPlanPayment", new { sessionId = transactionId, planid = plan.Id });
            //  return Redirect(url);
            return Ok()
;        }


        private string Pay(int planId, PaymentMethod method, string token)
        {
            // This is a simulated redirect URL — in real life this would be a Stripe session URL or internal route
            var encodedToken = Uri.EscapeDataString(token); // prevent issues with commas/special chars
            var url = $"/payments/checkout-subscription?planId={planId}&method={(int)method}&token={encodedToken}";
            return url;
        }



        [HttpGet("PaySubscriptionFromCard")]
        public async Task<IActionResult> PaySubscriptionFromCard(int planId, [FromQuery] CardPaymentDTO card)
        {
            //var userId = "4be59d6d-28bd-4a39-93eb-33f304792d84";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var freelancer = await _context.freelancers.FirstOrDefaultAsync(f => f.Id == userId);
            var client = await _context.clients.FirstOrDefaultAsync(c => c.Id == userId);

            if (freelancer == null && client == null)
                return BadRequest(new { Message = "User must be a freelancer or client." });

            var plan = SubscriptionPlansHelper.SubscriptionPlans.FirstOrDefault(p => p.Id == planId);
            if (plan == null)
                return BadRequest(new { Message = "Subscription plan not found." });

            if (plan.Price == 0)
                return BadRequest("This is a free plan. No card payment required.");
            // Deduct balance
            if (freelancer != null)
            {
               
                freelancer.subscriptionPlanId = plan.Id;
                freelancer.RemainingNumberOfBids = plan.TotalNumber;
            }
            else if (client != null)
            {
                
                client.subscriptionPlanId = plan.Id;
                client.RemainingNumberOfProjects = plan.TotalNumber;
            }

            // Record payment
            var cardToken = $"{card.Cardnumber},{card.cvv}"; // ⚠️ Never do this in production without secure handling
            var payment = new SubscriptionPayment
            {
                Amount = plan.Price,
                Date = DateTime.UtcNow,
                PaymentMethod = PaymentMethod.Balance,
                TransactionId =  cardToken, // Use same transactionId
                IsDeleted = false
            };

            _context.SubscriptionPayments.Add(payment);
            await _context.SaveChangesAsync();

            // Link payment to user and plan
            var userPlan = new UserSubscriptionPlanPayment
            {
                UserId = userId,
                SubscriptionPlanId = plan.Id,
                SubscriptionPaymentId = payment.Id,
                isDeleted = false
            };

            _context.UserSubscriptionPlanPayments.Add(userPlan);
            await _context.SaveChangesAsync();
            // Combine card details into a token or pass-through string (simulate)

            // Build a payment redirect URL (like your `Pay()` function in the proposal)
            var url = Pay(planId, PaymentMethod.CreditCard, cardToken);

            // You can optionally log this attempt or store a pending payment record

            // Simulate a redirect or just return success
            return Ok(new
            {
                MessageProcessingHandler = "Payment processing started.",
                RedirectUrl = url,
                PlanId = planId,
                PaymentMethod = PaymentMethod.CreditCard,
                CardDetails = new
                {
                    CardNumber = card.Cardnumber,
                    Cvv = card.cvv
                },
                Amount = plan.Price,
                UserId = userId,
              

            });
        }





        [HttpGet("PaySubscriptionFromStripe")]
        public async Task<IActionResult> PaySubscriptionFromStripe(int planId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            // var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized(new { Message = "User not authenticated." });

            var client = await _context.clients.FirstOrDefaultAsync(c => c.Id == userId);
            var freelancer = await _context.freelancers.FirstOrDefaultAsync(f => f.Id == userId);

            if (client == null && freelancer == null)
                return BadRequest(new { Message = "User must be a client or freelancer." });

            var plan = SubscriptionPlansHelper.SubscriptionPlans.FirstOrDefault(p => p.Id == planId);
            if (plan == null)
                return BadRequest(new { Message = "Subscription plan not found." });

            if (plan.Price == 0)
                return BadRequest(new { Message = "This is a free plan — Stripe payment not required." });

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var successUrl = $"{baseUrl}/api/SubscribtionPlanPayment/payment-success?sessionId={{CHECKOUT_SESSION_ID}}&planId={planId}";

            // Pass control to your Stripe controller's CreateCheckoutSession endpoint
            var stripeUrl = Url.ActionLink("CreateCheckoutSession", "Stripe", new
            {
                Amount = plan.Price,
                redirectionurl = successUrl
            });

            if (string.IsNullOrEmpty(stripeUrl))
                return StatusCode(500, "Could not generate Stripe checkout URL.");

            return Redirect(stripeUrl);
        }










        [HttpGet("payment-success")]
        public async Task <IActionResult> paymentsucess(string sessionId, int planid)
        {
            try
            {

                var sessionService = new SessionService();
                var session = sessionService.Get(sessionId, new SessionGetOptions
                {
                    Expand = new List<string> { "line_items.data.price.product" }
                });
                if (!session.Metadata.TryGetValue("userId", out var userId))
                {
                    // This userId is unique per session
                    return BadRequest(new { Message = "payment failed" });
                }



                var freelancer = await _context.freelancers.FirstOrDefaultAsync(f => f.Id == userId);
                var client = await _context.clients.FirstOrDefaultAsync(c => c.Id == userId);

              //  if (freelancer == null && client == null)
                //    return BadRequest("User must be a freelancer or client.");

                var plan = SubscriptionPlansHelper.SubscriptionPlans.FirstOrDefault(p => p.Id == planid);
                if (plan == null)
                    return BadRequest("Subscription plan not found.");

                if (plan.Price == 0)
                    return BadRequest("This is a free plan. No card payment required.");
                // Deduct balance
                if (freelancer != null)
                {

                    freelancer.subscriptionPlanId = plan.Id;
                    freelancer.RemainingNumberOfBids = plan.TotalNumber;
                }
                else if (client != null)
                {

                    client.subscriptionPlanId = plan.Id;
                    client.RemainingNumberOfProjects = plan.TotalNumber;
                }

                // Record payment
                var payment = new SubscriptionPayment
                {
                    Amount = plan.Price,
                    Date = DateTime.UtcNow,
                    PaymentMethod = PaymentMethod.Stripe,
                    TransactionId = sessionId, // Use same transactionId
                    IsDeleted = false
                };

                _context.SubscriptionPayments.Add(payment);
                await _context.SaveChangesAsync();

                // Link payment to user and plan
                var userPlan = new UserSubscriptionPlanPayment
                {
                    UserId = userId,
                    SubscriptionPlanId = plan.Id,
                    SubscriptionPaymentId = payment.Id,
                    isDeleted = false
                };

                _context.UserSubscriptionPlanPayments.Add(userPlan);
                await _context.SaveChangesAsync();
                // Combine card details into a token or pass-through string (simulate)

                // Build a payment redirect URL (like your `Pay()` function in the proposal)
                var url = Pay(planid, PaymentMethod.Stripe,sessionId);






                if (session == null)
                {
                    return BadRequest(new { Message = "Session not found." });
                }
                // Handle successful payment here (e.g., update user subscription status)

                // You can also redirect to the success URL if needed
                return Redirect($"http://localhost:4200/paymentsucess?sessionId={sessionId}");
                return Ok(new { Message = "Payment successful.", session });
              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during payment success handling");
                return StatusCode(500, new { error = "An error occurred while processing the payment." });
            }
        }
    }
}
