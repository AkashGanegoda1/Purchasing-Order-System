using System;
using System.Collections.Generic;

namespace PDSA_CW
{
    // Class representing an order
    class Order
    {
        // Declaring Variables

        public int OrderId;
        public string Supplier;
        public string Address;
        public int Vat;
        public string ProductName;
        public int Quantity;
        public float Price;
        public DateTime Date;
        public float Total;



        // Constructor for creating an order object
        public Order(int orderId, string supplier, string address, int vat, string productName, int quantity, float price)
        {
            OrderId = orderId;
            Supplier = supplier;
            Address = address;
            Vat = vat;
            ProductName = productName;
            Quantity = quantity;
            Price = price;
            Date = DateTime.Now;       // Current date and time
            Total = CalculateTotal();  // Calculated total cost including VAT
        }


        // Method to calculate the total cost of the order including VAT
        private float CalculateTotal()
        {
            float subtotal = Quantity * Price;
            float vatAmount = (subtotal * Vat) / 100;
            return subtotal + vatAmount;
        }
    }


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

            // Check if the order ID already exists
            if (OrderExists(order.OrderId))
            {
                Console.WriteLine("Order ID already exists!");
                return;
            }

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
            if (order.OrderId < node.Order.OrderId)
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
            else if (order.OrderId > node.Order.OrderId)
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
            if (orderId < node.Order.OrderId)
                node.Left = UpdateOrders(node.Left, orderId, newOrder);

            // If the orderId to update is greater than the current node's orderId, go right
            else if (orderId > node.Order.OrderId)
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
            if (orderId < node.Order.OrderId)
                node.Left = DeleteOrders(node.Left, orderId);

            //If the order ID to delete is greater than the current node's order ID, go to the right node.
            else if (orderId > node.Order.OrderId)
                node.Right = DeleteOrders(node.Right, orderId);
            else
            {
                if (node.Left == null)
                    return node.Right; // Return right child if left child is null

                else if (node.Right == null)
                    return node.Left; // Return left child if right child is null

                TreeNode nextnode = MinValue(node.Right); // Node has two children and find the nextnode

                node.Order = nextnode.Order; // Replace the node's data with the nextnode's data

                node.Right = DeleteOrders(node.Right, node.Order.OrderId); // Delete the nextnode's data
            }

            return node; // Return the node
        }


        private TreeNode MinValue(TreeNode node) //  method to find the node with the minimum value in the subtree
        {
            while (node.Left != null)
            {
                node = node.Left;
            }
            return node;
        }
        public bool OrderExists(int orderId) // Method to check if an order with a given ID exists
        {
            return OrderExists(root, orderId);
        }

        private bool OrderExists(TreeNode node, int orderId) // Private method to check if an order with a given ID exists
        {
            if (node == null)
                return false;

            if (orderId < node.Order.OrderId)
                return OrderExists(node.Left, orderId);
            else if (orderId > node.Order.OrderId)
                return OrderExists(node.Right, orderId);
            else
                return true;
        }
        public void DisplayOrderss()     // Method to display all orders in the binary tree
        {
            if (root == null)
            {
                Console.WriteLine("No orders to display.");
            }
            else
            {
                Display(root);
            }
        }

        private void Display(TreeNode node)
        {
            if (node != null)
            {
                Display(node.Left);
                Console.WriteLine($"Date & time: {node.Order.Date}\nOrder ID: {node.Order.OrderId}\nSupplier: {node.Order.Supplier}\nAddress: {node.Order.Address}\nProduct: {node.Order.ProductName}\nQuantity: {node.Order.Quantity}\nPrice: {node.Order.Price}\nVat: {node.Order.Vat}\nTotal: {node.Order.Total}\n");
                Display(node.Right);
            }
        }
        public void DisplayOrders()         // Method to display all orders in the sorted order
        {
            if (sortedOrders.Count == 0)
            {
                Console.WriteLine("No orders to display.");
                return;
            }

            Console.WriteLine("Orders: ");
            foreach (var order in sortedOrders)
            {
                Console.WriteLine($"Date & time: {order.Date}\nOrder ID: {order.OrderId}\nSupplier: {order.Supplier}\nAddress: {order.Address}\nProduct: {order.ProductName}\nQuantity: {order.Quantity}\nPrice: {order.Price}\nVat: {order.Vat}\nTotal: {order.Total}\n");
            }
        }

        public void SortOrders(bool ascending)   // Method to sort orders based on total cost
        {
            sortedOrders.Clear();
            GetOrdersInOrder(root, sortedOrders);

            if (ascending)
            {
                sortedOrders.Sort((x, y) => x.Total.CompareTo(y.Total));
            }
            else
            {
                sortedOrders.Sort((x, y) => y.Total.CompareTo(x.Total));
            }
        }

        private void GetOrdersInOrder(TreeNode node, List<Order> orders)
        {
            if (node != null)
            {
                GetOrdersInOrder(node.Left, orders);
                orders.Add(node.Order);
                GetOrdersInOrder(node.Right, orders);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            POOrderSystem orderSystem = new POOrderSystem(); // Creating an object of the class

            Console.WriteLine("-----------------------------");
            Console.WriteLine("Purchasing Order System (POS)");
            Console.WriteLine("-----------------------------");

            bool exit = false;

            while (!exit)   // Displaying options
            {
                Console.WriteLine("\nOptions:\n");
                Console.WriteLine("1. Add Order");
                Console.WriteLine("2. Update Order");
                Console.WriteLine("3. Delete Order");
                Console.WriteLine("4. Display Orders");
                Console.WriteLine("5. Sort Orders from Cheapest to Most Expensive");
                Console.WriteLine("6. Sort Orders from Most Expensive to Cheapest");
                Console.WriteLine("7. Exit");
                Console.Write("\nSelect an option: ");
                string input = Console.ReadLine();

                switch (input) // Handling the user inputs by using Switch
                {
                    case "1":
                        Console.WriteLine("\nEnter Order details");
                        Console.WriteLine("*******************\n");

                        int orderId;
                        while (true)
                        {
                            Console.Write("Order ID: ");
                            string inputs = Console.ReadLine(); // Taking user inputs

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
                                break; // Break the loop if one of the above cases triggered
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
                        break;

                    case "2":
                        // Updating an existing order
                        Console.WriteLine("\nUpdate Order");
                        Console.WriteLine("************\n");
                        Console.Write("Enter the Order ID to update: ");
                        int updateOrderId = int.Parse(Console.ReadLine());
                        if (orderSystem.OrderExists(updateOrderId))
                        {
                            Console.WriteLine("\nEnter order details to Update");
                            Console.WriteLine("******************************\n");

                            Console.Write("Supplier: ");
                            string updatedSupplier = Console.ReadLine();
                            Console.Write("Address: ");
                            string updatedAddress = Console.ReadLine();
                            Console.Write("VAT (%): ");
                            int updatedVat;
                            while (!int.TryParse(Console.ReadLine(), out updatedVat) || updatedVat < 0 || updatedVat > 100)
                            {
                                Console.WriteLine("Invalid VAT percentage. Please enter a value between 0 and 100.");
                                Console.Write("VAT (%): ");
                            }

                            Console.Write("Product Name: ");
                            string updatedProductName = Console.ReadLine();
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

                            Order updatedOrder = new Order(updateOrderId, updatedSupplier, updatedAddress, updatedVat, updatedProductName, updatedQuantity, updatedPrice);
                            orderSystem.UpdateOrder(updateOrderId, updatedOrder);
                        }
                        else
                        {
                            Console.WriteLine("\nOrder ID does not exist.");
                        }
                        break;

                    case "3":
                        // Deleting an existing order
                        Console.WriteLine("\nDelete Order");
                        Console.WriteLine("************");
                        Console.Write("\nEnter the Order ID to delete: ");
                        int deleteOrderId = int.Parse(Console.ReadLine());
                        if (orderSystem.OrderExists(deleteOrderId))
                        {
                            orderSystem.DeleteOrder(deleteOrderId);
                        }
                        else
                        {
                            Console.WriteLine("\nOrder ID does not exist.");
                        }
                        break;

                    case "4":
                        // Displaying all orders
                        Console.WriteLine("\nOrder/s Summary");
                        Console.WriteLine("***************\n");
                        orderSystem.DisplayOrderss();
                        break;

                    case "5":
                        // Sorting orders from cheapest to most expensive
                        orderSystem.SortOrders(true);
                        Console.WriteLine("\nOrders sorted from Cheapest to Most Expensive:\n");
                        orderSystem.DisplayOrders();
                        break;

                    case "6":
                        // Sorting orders from most expensive to cheapest
                        orderSystem.SortOrders(false);
                        Console.WriteLine("\nOrders sorted from Most Expensive to Cheapest:\n");
                        orderSystem.DisplayOrders();
                        break;

                    case "7":
                        // Exiting the program
                        exit = true;
                        Console.WriteLine("\nExiting...");
                        break;

                    default:
                        Console.WriteLine("\nInvalid option. Please select a valid option (1-7).");  // Invalid option
                        break;
                }
            }
        }
    }
}
