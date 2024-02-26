using System;
using System.Collections.Generic;

namespace PDSA_CW
{
    class Order   // Class representing an order
    {
        // Declaring Variables

        public int OrderId; // Unique identifier for the order
        public string Supplier; // Name of the supplier
        public string Address;  // Address of the Supplier
        public int Vat; // VAT percentage
        public string ProductName; // Name of the product
        public int Quantity; //Quantity of the product ordered
        public float Price; // Price per unit of the product
        public DateTime Date;  // Date and time of the order
        public float Total; // Total cost of the order

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
            Date = DateTime.Now; // Current date and time
            Total = CalculateTotal();  // Calculate total cost including VAT
        }

        private float CalculateTotal()  // Method to calculate the total cost of the order including VAT
        {
            float subtotal = Quantity * Price;
            float vatAmount = (subtotal * Vat) / 100;
            return subtotal + vatAmount;
        }
    }

    class TreeNode
    {
        // Class representing a node in a binary search tree
        public Order Order; // Order associated with the node
        public TreeNode Left; // Reference to the left child node
        public TreeNode Right; // Reference to the right child node

        public TreeNode(Order order)   // Constructor for creating a tree node 
        {
            Order = order;
            Left = null;
            Right = null;
        }
    }

    class POOrderSystem    // Class representing a purchasing order system
    {
        private TreeNode root;   // Root node of the binary search tree
        private List<Order> sortedOrders;  // List to store sorted orders

        public POOrderSystem()  // Constructor for creating a new purchasing order system
        {
            sortedOrders = new List<Order>();
        }

        public void AddOrder(Order order)
        {
            if (OrderExists(order.OrderId))
            {
                Console.WriteLine("Order ID already exists!");   
                return;
            }

            if (root == null)
            {
                root = new TreeNode(order);
            }
            else
            {
                AddOrders(root, order);
            }
        }

        private void AddOrders(TreeNode node, Order order)
        {
            if (order.OrderId < node.Order.OrderId)
            {
                if (node.Left == null)
                {
                    node.Left = new TreeNode(order);
                }
                else
                {
                    AddOrders(node.Left, order);
                }
            }
            else if (order.OrderId > node.Order.OrderId)
            {
                if (node.Right == null)
                {
                    node.Right = new TreeNode(order);
                }
                else
                {
                    AddOrders(node.Right, order);
                }
            }
        }

        public void UpdateOrder(int orderId, Order newOrder) // update order method

        {
            if (!OrderExists(orderId))
            {
                Console.WriteLine("Order with ID " + orderId + " does not exist.");
                return;
            }

            root = UpdateOrders(root, orderId, newOrder);
            Console.WriteLine("Order Updated Successfully");

        }

        private TreeNode UpdateOrders(TreeNode node, int orderId, Order newOrder)
        {
            if (node == null)
                return null;

            if (orderId < node.Order.OrderId)
                node.Left = UpdateOrders(node.Left, orderId, newOrder);
            else if (orderId > node.Order.OrderId)
                node.Right = UpdateOrders(node.Right, orderId, newOrder);
            else
                node.Order = newOrder;

            return node;
        }

        public void DeleteOrder(int orderId)
        {
            if (!OrderExists(orderId))
            {
                Console.WriteLine("Order with ID " + orderId + " does not exist.");
                return;
            }

            root = DeleteOrders(root, orderId);
            Console.WriteLine("Order Deleted Successfully");

        }

        private TreeNode DeleteOrders(TreeNode node, int orderId) // deleting orders using tree node
        {
            if (node == null)
                return null;

            if (orderId < node.Order.OrderId)
                node.Left = DeleteOrders(node.Left, orderId);
            else if (orderId > node.Order.OrderId)
                node.Right = DeleteOrders(node.Right, orderId);
            else
            {
                if (node.Left == null)
                    return node.Right;
                else if (node.Right == null)
                    return node.Left;

                TreeNode successor = MinValue(node.Right);
                node.Order = successor.Order;
                node.Right = DeleteOrders(node.Right, node.Order.OrderId);
            }

            return node;
        }

        private TreeNode MinValue(TreeNode node)
        {
            while (node.Left != null)
            {
                node = node.Left;
            }
            return node;
        }
        public bool OrderExists(int orderId)
        {
            return OrderExists(root, orderId);
        }

        private bool OrderExists(TreeNode node, int orderId)
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
        public void DisplayOrderss()
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
        public void DisplayOrders()
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

        public void SortOrders(bool ascending)
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

            while (!exit)
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
                        Console.WriteLine("\nOrder/s Summary");
                        Console.WriteLine("***************\n");
                        orderSystem.DisplayOrderss();
                        break;

                    case "5":
                        orderSystem.SortOrders(true);
                        Console.WriteLine("\nOrders sorted from Cheapest to Most Expensive:\n");
                        orderSystem.DisplayOrders();
                        break;

                    case "6":
                        orderSystem.SortOrders(false);
                        Console.WriteLine("\nOrders sorted from Most Expensive to Cheapest:\n");
                        orderSystem.DisplayOrders();
                        break;

                    case "7":
                        exit = true;
                        Console.WriteLine("\nExiting...");
                        break;

                    default:
                        Console.WriteLine("\nInvalid option. Please select a valid option (1-7).");
                        break;
                }
            }
        }
    }
}
