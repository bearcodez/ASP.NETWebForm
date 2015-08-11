# ASP.NETWebForm

This project is a simple customize shopping cart solution. In the web page ProductMachines.aspx, all the product machines are listed into a repeater and then tranform into HTML table. When user wants to buy a certain product on the listing page, they can click on the "Add to Cart" button and a shopping cart item record will be stored in database for future use.

Code Explain
ProductMachines.aspx.cs/btnAddToCartDetails_Click - This method will add the selected product into shopping cart record in database.


User can view their current shopping cart by going to MyCart.aspx page. All their selected products will be shown on this page. They can choose to remove or update existing products in their cart. As soon as they are ready to purchase, they can click the "Check Out" button to proceed to the booking process.

Code Explain
MyCart.aspx.cs/DisplayCarts - This method display cart details such as total price and quantity for all the products selected.

MyCart.aspx.cs/BindDataMachine - This method bind data to repeater item by looping each record retrieve from database.

MyCart.aspx.cs/repMachine_ItemCommand - This method used to perform click event such as update quantity and remove product from shopping cart

MyCart.aspx.cs/UpdateCart - This method update the shopping cart record and save into database.

MyCart.aspx.cs/RemoveCart - This method remove the shopping cart record from database.

MyCart.aspx.cs/btnCheckOut_Click - This method will perform checking on expired items and redirect to CheckOut page.
