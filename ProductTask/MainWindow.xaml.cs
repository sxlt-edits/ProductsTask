using ProductTask.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProductTask.Forms;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.Drawing.Imaging;
using System.IO;

namespace ProductTask
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Product> ListProduct { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            ListProduct = new();
            DataContext = this;
            this.Loaded += DatabaseContext_Loaded;
        }
        private void DatabaseContext_Loaded(object sender, RoutedEventArgs e)
        {
            DatabaseContext databaseContext = new DatabaseContext();
            databaseContext.Database.Migrate();
            List<Product> products = databaseContext.Products.ToList();
            ProductList.ItemsSource = products;

            //QRCode

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            foreach (Product product in products)
            {
                string combined = "Unique ID: " + product.ID + "\r\n" + "Name: " + product.Name + "\r\n" + "Description: " + product.Description + "\r\n" + "Price: " + product.Price;
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(combined, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                BitmapImage qrCodeImage = new BitmapImage();
                using (MemoryStream stream = new MemoryStream())
                {
                    qrCode.GetGraphic(20).Save(stream, ImageFormat.Png);
                    stream.Seek(0, SeekOrigin.Begin);
                    qrCodeImage.BeginInit();
                    qrCodeImage.CacheOption = BitmapCacheOption.OnLoad;
                    qrCodeImage.StreamSource = stream;
                    qrCodeImage.EndInit();
                }

                ListProduct.Add(new Product { Name = product.Name, Price = product.Price, QRCode = qrCodeImage, Description = product.Description, ID = product.ID });
            }
            ProductList.ItemsSource = ListProduct;
        }
            //Button add

            private void btn_add_Click(object sender, RoutedEventArgs e)
            {
            Forms.Dialog add = new Forms.Dialog();
            Close();
            add.ShowDialog();
            }

            //Button edit
            private void btn_edit_Click(object sender, RoutedEventArgs e)
            {
            if (ProductList.SelectedItem != null)
            {

                var product = ProductList.SelectedItem as Product;
                if (new Forms.Edit(product).ShowDialog() == true)
                {
                    using (var context = new DatabaseContext())
                    {
                        context.Entry(product).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                    ProductList.Items.Refresh();
                }
            }
            MainWindow mainWindow = new MainWindow();
            Close();
            mainWindow.ShowDialog();
            }
            //Button remove

            private void btn_remove_Click(object sender, RoutedEventArgs e)
            {

                if (ProductList.SelectedItem != null)
                {
                        var product = ProductList.SelectedItem as Product;
                        using (var context = new DatabaseContext())
                        {
                            context.Products.Remove(product);
                            context.SaveChanges();
                            ProductList.ItemsSource = context.Products.ToList();
                        }
                }
                MainWindow mainWindow = new MainWindow();
                Close();
                mainWindow.ShowDialog();
            }
            
            //Button exit

            private void btn_exit_Click(object sender, RoutedEventArgs e)
            {
                Application.Current.Shutdown();
            }
    }       
    
}
