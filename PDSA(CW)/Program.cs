using System;
using System.Collections.Generic;


namespace PDSA_CW
{
    // Class representing an order
    class Order
    {
        // Declaring Variables

        private int OrderId;
        private string Supplier;
        private string Address;
        private int Vat;
        private string ProductName;
        private int Quantity;
        private float Price;
        private DateTime Date;
        private float Total;



        // Constructor for creating an order object
        public Order(int orderId, string supplier, string address, int vat, string productName, int quantity, float price)
        {
            this.OrderId = orderId;
            this.Supplier = supplier;
            this.Address = address;
            this.Vat = vat;
            this.ProductName = productName;
            this.Quantity = quantity;
            this.Price = price;
            this.Date = DateTime.Now;       // Current date and time
            this.Total = CalculateTotal();  // Calculated total cost including VAT
        }

        // Method to calculate the total cost of the order including VAT
        private float CalculateTotal()
        {
            float subtotal = Quantity * Price;
            float vatAmount = (subtotal * Vat) / 100;
            return subtotal + vatAmount;
        }

        // Get methods to access private variables
        public int GetOrderId()
        {
            return OrderId;
        }

        public string GetSupplier()
        {
            return Supplier;
        }

        public string GetAddress()
        {
            return Address;
        }

        public int GetVat()
        {
            return Vat;
        }

        public string GetProductName()
        {
            return ProductName;
        }

        public int GetQuantity()
        {
            return Quantity;
        }

        public float GetPrice()
        {
            return Price;
        }

        public DateTime GetDate()
        {
            return Date;
        }

        public float GetTotal()
        {
            return Total;
        }
    }

    // Class representing a node in the binary search tree
    class TreeNode
    {

        public Order Order; // Order associated with the node
        public TreeNode Left; // Reference to the left child node
        public TreeNode Right; // Reference to the right child node


        // Constructor for creating a tree node 
        public TreeNode(Order order)
        {
            Order = order;
            Left = null;
            Right = null;
        }
    }

    // Class representing a purchasing order
    class POOrderSystem
    {
        private TreeNode root;   // Root node of the binary search tree
        private List<Order> sortedOrders;  // List to store sorted orders


        // Constructor for creating a new purchasing order
        public POOrderSystem()
        {
            sortedOrders = new List<Order>();
        }

        // Method to add a new order to binary tree
        public void AddOrder(Order order)
        {
            // if the root is empty then making the order as root else add the order normally
            if (root == null)
            {
                root = new TreeNode(order);
            }
            else
            {
                AddOrders(root, order);
            }
        }


        // Private method to add orders to the binary tree
        private void AddOrders(TreeNode node, Order order)
        {

            // if the orderid in the Order class is less than the orderid in Tree node class, order will be added to the left
            if (order.GetOrderId() < node.Order.GetOrderId())
            {

                // if the left node is empty add the order to the left node
                if (node.Left == null)
                {
                    // Add the Order as a new left child TreeNode
                    node.Left = new TreeNode(order);
                }
                else
                {
                    // Otherwise AddOrders on the left subtree
                    AddOrders(node.Left, order);
                }
            }

            // if the orderid in the Order class is Greater than the orderid in Tree node class, order will be added to the Right
            else if (order.GetOrderId() > node.Order.GetOrderId())
            {
                // If the right child node of the current TreeNode is empty
                if (node.Right == null)
                {
                    // Add the Order as a new right child TreeNode
                    node.Right = new TreeNode(order);
                }
                else
                {
                    // Otherwise AddOrders on the right subtree
                    AddOrders(node.Right, order);
                }
            }
        }

        // Method to update an existing order
        public void UpdateOrder(int orderId, Order newOrder)

        {
            root = UpdateOrders(root, orderId, newOrder); // Call the UpdateOrders starting from the root
        }


        // Private method to update orders in the binary tree
        private TreeNode UpdateOrders(TreeNode node, int orderId, Order newOrder)
        {

            //If the node is null return null
            if (node == null)
                return null;

            // If the orderId to update is less than the current node's orderId, go left
            if (orderId < node.Order.GetOrderId())
                node.Left = UpdateOrders(node.Left, orderId, newOrder);

            // If the orderId to update is greater than the current node's orderId, go right
            else if (orderId > node.Order.GetOrderId())
                node.Right = UpdateOrders(node.Right, orderId, newOrder);
            else
                node.Order = newOrder; // Update the order details with the newOrder provided

            return node; // Return the updated node
        }

        public void DeleteOrder(int orderId)  // Method to delete an existing order
        {
            root = DeleteOrders(root, orderId);
        }

        private TreeNode DeleteOrders(TreeNode node, int orderId)  // Private method to delete orders from the binary tree
        {
            //If the node is null, return null
            if (node == null)
                return null;

            // If the order ID to delete is less than the current node's order ID, go to the left node.
            if (orderId < node.Order.GetOrderId())
                node.Left = DeleteOrders(node.Left, orderId);

            //If the order ID to delete is greater than the current node's order ID, go to the right node.
            else if (orderId > node.Order.GetOrderId())
                node.Right = DeleteOrders(node.Right, orderId);
            else
            {
                if (node.Left == null)
                    return node.Right; // Return right child if left child is null

                else if (node.Right == null)
                    return node.Left; // Return left child if right child is null

                TreeNode nextnode = MinValue(node.Right); // Node has two children and find the nextnode

                node.Order = nextnode.Order; // Replace the node's data with the nextnode's data

                node.Right = DeleteOrders(node.Right, nextnode.Order.GetOrderId()); // Delete the nextnode's data
            }

            return node; // Return the node
        }


        // Method to find the node with the minimum value in the subtree
        private TreeNode MinValue(TreeNode node)
        {

            // While there is a left child node, keep looking to the left to find the minimum value
            while (node.Left != null)
            {
                node = node.Left; // Move to the left child node
            }
            return node; // Return the node with the minimum value
        }

        // Method to check if an order with a given ID exists
        public bool OrderExists(int orderId)
        {
            return OrderExistCheck(root, orderId);
        }


        // Private method to check if an order with a given ID exists
        private bool OrderExistCheck(TreeNode node, int orderId)
        {

            // If the current node is null, the order isn't exist in this subtree so return false
            if (node == null)
                return false;

            // Compare the orderId of the current node with the given orderId
            if (orderId < node.Order.GetOrderId())

                // If the given orderId is less search in the left subtree
                return OrderExistCheck(node.Left, orderId);

            else if (orderId > node.Order.GetOrderId())
                // If the given orderId is greater search in the right subtree
                return OrderExistCheck(node.Right, orderId);

            else
                // If the orderId matches the current node's orderId, the order exists so return true
                return true;
        }

        // Method to display all orders in the binary tree
        public void DisplayOrders()
        {

            // Check if the root node is null
            if (root == null)
            {
                Console.WriteLine("No orders to display.");
            }
            else
            {
                SetDisplayData(root); // If the root is not empty, call the private Display method starting from the root
            }
        }


        // Method to recursively display orders in the binary tree in an in-order traversal
        private void SetDisplayData(TreeNode node)
        {
            // If the current node is not null
            if (node != null)
            {
                SetDisplayData(node.Left); // display orders in the left subtree

                // Display the order details for the current node
                Console.WriteLine($"Date & time: {node.Order.GetDate()}\nOrder ID: {node.Order.GetOrderId()}\nSupplier: {node.Order.GetSupplier()}\nAddress: {node.Order.GetAddress()}\nProduct: {node.Order.GetProductName()}\nQuantity: {node.Order.GetQuantity()}\nPrice: {node.Order.GetPrice()}\nVat: {node.Order.GetVat()}\nTotal: {node.Order.GetTotal()}\n");

                SetDisplayData(node.Right); // display orders in the right subtree
            }
        }

        // Method to display all orders in the sorted order
        public void DisplaySortedOrders()
        {
            // Check if there are no orders in the sorted list
            if (sortedOrders.Count == 0)
            {
                Console.WriteLine("No orders to display.");
                return;
            }

            Console.WriteLine("Orders: ");

            // Going through each order in the sorted list
            foreach (var order in sortedOrders)
            {
                // Display the order details for each order
                Console.WriteLine($"Date & time: {order.GetDate()}\nOrder ID: {order.GetOrderId()}\nSupplier: {order.GetSupplier()}\nAddress: {order.GetAddress()}\nProduct: {order.GetProductName()}\nQuantity: {order.GetQuantity()}\nPrice: {order.GetPrice()}\nVat: {order.GetVat()}\nTotal: {order.GetTotal()}\n");
            }
        }

        // Method to sort orders based on total cost
        public void SortOrders(bool ascending)
        {

            // Clear the sortedOrders list to be sure it's empty before sorting
            sortedOrders.Clear();

            // sortedOrders list with orders using in-order traversal
            GetOrdersInOrder(root, sortedOrders);

            // Sort the orders based on total cost in ascending or descending order
            if (ascending)
            {
                // Sort in ascending order
                sortedOrders.Sort((x, y) => x.GetTotal().CompareTo(y.GetTotal()));
            }
            else
            {
                // Sort in descending order
                sortedOrders.Sort((x, y) => y.GetTotal().CompareTo(x.GetTotal()));
            }
        }

        private void GetOrdersInOrder(TreeNode node, List<Order> orders)
        {
            // Check if the current node is not null
            if (node != null)
            {
                // traverse the left subtree
                GetOrdersInOrder(node.Left, orders);

                // Add the order associated with the current node to the list
                orders.Add(node.Order);

                // traverse the right subtree
                GetOrdersInOrder(node.Right, orders);
            }
        }

        // Get the count of orders
        public int OrderCount()
        {
            return CountOrders(root);
        }

        // Count orders in the binary tree
        private int CountOrders(TreeNode node)
        {
            if (node == null)
                return 0;

            return 1 + CountOrders(node.Left) + CountOrders(node.Right);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear(); // Clear console

            // Creating an object of the puchase order system class
            POOrderSystem orderSystem = new POOrderSystem();

            //Header for the Purchasing Order System
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Purchasing Order System (POS)");
            Console.WriteLine("-----------------------------");

            bool exit = false;
            bool ordersAdded = false; // track whether orders are added or not

            // Loop to continuously display options until the user choose to exit
            while (!exit)
            {

                // Displaying options
                Console.WriteLine("\nOptions:\n");
                Console.WriteLine("1. Add Order");
                Console.WriteLine("2. Update Order");
                Console.WriteLine("3. Delete Order");
                Console.WriteLine("4. Display Orders");
                Console.WriteLine("5. Sort Orders from Cheapest to Most Expensive");
                Console.WriteLine("6. Sort Orders from Most Expensive to Cheapest");
                Console.WriteLine("7. Exit");
                Console.Write("\nSelect an option: ");

                // Reading user input from the options
                string input = Console.ReadLine();

               

                // Switch statement to handle user selected options
                switch (input)
                {
                    case "1":

                        // Adding a Order Process
                        Console.WriteLine("\nEnter Order details");
                        Console.WriteLine("*******************\n");
                        int orderId;
                        while (true)
                        {
                            Console.Write("Order ID: ");
                            string inputs = Console.ReadLine();

                            // Validations
                            if (string.IsNullOrEmpty(inputs))
                            {
                                Console.WriteLine("Order ID cannot be null or empty.");
                            }
                            else if (!int.TryParse(inputs, out orderId) || orderId <= 0)
                            {
                                Console.WriteLine("Order ID must be a positive integer.");
                            }
                            else if (orderSystem.OrderExists(orderId))
                            {
                                Console.WriteLine("Order ID already exists.");
                            }
                            else
                            {
                                break; // Break the loop if none of the above cases triggered
                            }
                        }

                        string supplier, address, productName; // Declaring Variables

                        do
                        {
                            Console.Write("Supplier: ");
                            supplier = Console.ReadLine();

                            // Validate if the input is null or empty
                            if (string.IsNullOrEmpty(supplier))
                            {
                                Console.WriteLine("Supplier cannot be null or empty.");
                            }

                        } while (string.IsNullOrEmpty(supplier)); // While the input is null or empty user should enter values again

                        do
                        {
                            Console.Write("Address: ");
                            address = Console.ReadLine();

                            if (string.IsNullOrEmpty(address))
                            {
                                Console.WriteLine("Address cannot be null or empty.");
                            }

                        } while (string.IsNullOrEmpty(address));

                        Console.Write("VAT (%): ");
                        int vat;
                        while (!int.TryParse(Console.ReadLine(), out vat) || vat < 0 || vat > 100)
                        {
                            Console.WriteLine("Invalid VAT percentage. Please enter a value between 0 and 100.");
                            Console.Write("VAT (%): ");
                        }

                        do
                        {
                            Console.Write("Product Name: ");
                            productName = Console.ReadLine();
                            if (string.IsNullOrEmpty(productName))
                            {
                                Console.WriteLine("Product Name cannot be null or empty.");
                            }

                        } while (string.IsNullOrEmpty(productName));

                        Console.Write("Quantity: ");
                        int quantity;
                        while (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
                        {
                            Console.WriteLine("Quantity must be a positive integer.");
                            Console.Write("Quantity: ");
                        }

                        Console.Write("Price: ");
                        float price;
                        while (!float.TryParse(Console.ReadLine(), out price) || price <= 0)
                        {
                            Console.WriteLine("Price must be a positive number.");
                            Console.Write("Price: ");
                        }

                        Order order = new Order(orderId, supplier, address, vat, productName, quantity, price);
                        orderSystem.AddOrder(order);

                        ordersAdded = true; // Flag that takes the record that order added 

                        Console.WriteLine("\nOrder Added Successfully");
                        break;

                    case "2":
                        // Updating an existing order
                        if (ordersAdded && orderSystem.OrderCount() > 0)
                        {
                            Console.WriteLine("\nUpdate Order");
                            Console.WriteLine("************\n");
                            Console.Write("Enter the Order ID to update: ");
                            string updateOrderId = Console.ReadLine();

                            // Validations
                            if (string.IsNullOrEmpty(updateOrderId))
                            {
                                Console.WriteLine("Order ID cannot be null or empty.");
                            }
                            else if (!int.TryParse(updateOrderId, out orderId) || orderId <= 0)
                            {
                                Console.WriteLine("Order ID must be a positive integer.");
                            }
                            else if (orderSystem.OrderExists(orderId))
                            {
                                Console.WriteLine("\nEnter order details to Update");
                                Console.WriteLine("******************************\n");

                                string updatedsupplier, updatedaddress, updatedproductName; // Declaring Variables

                                do
                                {
                                    Console.Write("Supplier: ");
                                    updatedsupplier = Console.ReadLine();

                                    // Validate if the input is null or empty
                                    if (string.IsNullOrEmpty(updatedsupplier))
                                    {
                                        Console.WriteLine("Supplier cannot be null or empty.");
                                    }

                                } while (string.IsNullOrEmpty(updatedsupplier)); // While the input is null or empty user should enter values again

                                do
                                {
                                    Console.Write("Address: ");
                                    updatedaddress = Console.ReadLine();

                                    if (string.IsNullOrEmpty(updatedaddress))
                                    {
                                        Console.WriteLine("Address cannot be null or empty.");
                                    }

                                } while (string.IsNullOrEmpty(updatedaddress));
                                Console.Write("VAT (%): ");
                                int updatedVat;
                                while (!int.TryParse(Console.ReadLine(), out updatedVat) || updatedVat < 0 || updatedVat > 100)
                                {
                                    Console.WriteLine("Invalid VAT percentage. Please enter a value between 0 and 100.");
                                    Console.Write("VAT (%): ");
                                }

                                do
                                {
                                    Console.Write("Product Name: ");
                                    updatedproductName = Console.ReadLine();
                                    if (string.IsNullOrEmpty(updatedproductName))
                                    {
                                        Console.WriteLine("Product Name cannot be null or empty.");
                                    }

                                } while (string.IsNullOrEmpty(updatedproductName));


                                Console.Write("Quantity: ");
                                int updatedQuantity;
                                while (!int.TryParse(Console.ReadLine(), out updatedQuantity) || updatedQuantity <= 0)
                                {
                                    Console.WriteLine("Quantity must be a positive integer.");
                                    Console.Write("Quantity: ");
                                }

                                Console.Write("Price: ");
                                float updatedPrice;
                                while (!float.TryParse(Console.ReadLine(), out updatedPrice) || updatedPrice <= 0)
                                {
                                    Console.WriteLine("Price must be a positive number.");
                                    Console.Write("Price: ");
                                }

                                int ordid = Convert.ToInt32(updateOrderId);

                                Order updatedOrder = new Order(ordid, updatedsupplier, updatedaddress, updatedVat, updatedproductName, updatedQuantity, updatedPrice);
                                orderSystem.UpdateOrder(ordid, updatedOrder);
                                Console.WriteLine("\nOrder Updated Successfully");
                            }
                            else
                            {
                                Console.WriteLine("\nOrder ID does not exist.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nPlease add an order first!");
                        }
                       break;

                    case "3":
                        // Deleting an existing order
                        if(ordersAdded && orderSystem.OrderCount() > 0)
                        {
                            Console.WriteLine("\nDelete Order");
                            Console.WriteLine("************");
                            Console.Write("\nEnter the Order ID to delete: ");
                            string deleteOrderId = Console.ReadLine();

                            if (string.IsNullOrEmpty(deleteOrderId))
                            {
                                Console.WriteLine("Order ID cannot be null or empty.");
                            }
                            else if (!int.TryParse(deleteOrderId, out orderId) || orderId <= 0)
                            {
                                Console.WriteLine("Order ID must be a positive integer.");
                            }
                            else if (orderSystem.OrderExists(orderId))
                            {
                                int Doid = Convert.ToInt32(deleteOrderId);
                                orderSystem.DeleteOrder(Doid);
                                Console.WriteLine("\nOrder Deleted Successfully");
                            }
                            else
                            {
                                Console.WriteLine("\nOrder ID does not exist.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nPlease add an order first!");
                        }
                        break;

                    case "4":
                        // Displaying all orders
                        Console.WriteLine("\nOrder/s Summary");
                        Console.WriteLine("***************\n");
                        orderSystem.DisplayOrders();
                        break;

                    case "5":
                        // Sorting orders from cheapest to most expensive
                        orderSystem.SortOrders(true);
                        Console.WriteLine("\nOrders sorted from Cheapest to Most Expensive:\n");
                        orderSystem.DisplaySortedOrders();
                        break;

                    case "6":
                        // Sorting orders from most expensive to cheapest
                        orderSystem.SortOrders(false);
                        Console.WriteLine("\nOrders sorted from Most Expensive to Cheapest:\n");
                        orderSystem.DisplaySortedOrders();
                        break;

                    case "7":
                        // Exiting the program
                        Console.WriteLine("\nExiting...");
                        exit = true;
                        break;

                    default:
                        // Invalid option
                        Console.WriteLine("\nInvalid option. Please select a valid option (1-7).");
                        break;
                }
            }
        }
    }
}
