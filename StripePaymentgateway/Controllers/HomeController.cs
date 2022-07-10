using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;

namespace StripePaymentgateway.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CreateCheckoutSession(string amount,string email, string addr)
        {
            var options = new Stripe.Checkout.SessionCreateOptions
            {


                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {

                            UnitAmount = Convert.ToInt32(amount) * 100,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "order",
                                //Description = "Good",

                            },

                        },

                        Quantity = 1,

                    },
                },
                
                CustomerEmail = email,
                Mode = "payment",
                SuccessUrl = "https://www.tradetowin.com/transactionsuccess_stripe.asp?TID=[stripetransactionID]",
                CancelUrl = "https://www.tradetowin.com/transactionfailed_stripe.asp?error=[reason]",
                
            };

            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new HttpStatusCodeResult(303);
        }
        [HttpPost]
        public ActionResult GetPayment()
        {
            var service = new PaymentIntentService();
            var options = new PaymentIntentCreateOptions
            {
                Amount = 1099,
                SetupFutureUsage = "off_session",
                Currency = "usd",
            };
            var paymentIntent = service.Create(options);
            return Json(paymentIntent);
        }

        public ActionResult success()
        {

            return View();
        }

        public ActionResult cancel()
        {

            return View();
        }

    }
}