// Paypal.js

// Must have (hidden) divs with:
//   paypalClientId
//   paypalMode
// containing as innerText the respective data passed from the server in the ViewBag

// REQUIRES: JQUERY 3.1 !!

var clientInfo = {
    clientId: document.getElementById('paypalClientId').innerText,
    mode: document.getElementById('paypalMode').innerText,
};

paypal.Button.render({

    env: clientInfo.mode,

    client: {
        sandbox: clientInfo.clientId,
        production: clientInfo.clientId
    },

    // Show the buyer a 'Pay Now' button in the checkout flow
    commit: true,

    // payment() is called when the button is clicked
    payment: function (data, actions) {
        // Get JSON order information from server
        return $.get('/paypal/GetOrderJson?country=' + getCountry())
            .then(function (data) {
                data = '{"intent":"sale","payer":{"payment_method":"paypal"},"transactions":[{"amount":{"currency":"USD","total":10.00,"details":{"shipping":0.0,"subtotal":10.00,"tax":0.0}},"payee":{"email":"stieg_d@yahoo.com"},"description":"Deerfly Patches - 12 patches - Qty: 1","item_list":{"items":[{"name":"Deerfly Patches - 12 patches","quantity":1,"price":10.00,"sku":"1","currency":"USD"}]}}],"redirect_urls":{"return_url":"https://www.deerflypatches.com","cancel_url":"https://www.deerflypatches.com"}}';
                var payment = JSON.parse(data);
                debugger;
                // Make a call to the REST api to create the payment
                return actions.payment.create({ payment: payment });
            });
    },

    // onAuthorize() is called when the buyer approves the payment
    onAuthorize: function (data, actions) {
        return actions.payment.get()
            .then(function (paymentDetails) {
                var verifyData = {
                    paymentDetails: JSON.stringify(paymentDetails)
                };
                $.post("/ShoppingCart/VerifyAndSave", verifyData)
                    .then(function () {
                        // Execute the payment
                        return actions.payment.execute();
                    })
                    .then(function (data) {
                        // Show a success page to the buyer
                        window.location.href = "/ShoppingCart/OrderSuccess";
                    })
                    .catch(function (data) {
                        alert('Error processing order: \n' + data.responseJSON.message);
                    });
            });
    }

}, '#paypal-button-container');
