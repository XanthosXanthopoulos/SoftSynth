using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

namespace PDADesktop
{
    public class ProductPageViewModel : BaseViewModel
    {
        #region Private Members
        
        private int selectedGroup;

        #endregion

        #region Public Properties

        public ObservableCollection<ProductGroup> ProductGroups { get; set; }
        
        [JsonIgnore]
        public int SelectedGroup
        {
            get
            {
                return selectedGroup;
            }

            set
            {
                if (selectedGroup != value)
                {
                    selectedGroup = value;
                    OnPropertyChanged(nameof(SelectedGroup));
                    OnPropertyChanged(nameof(SelectedProduct));
                }
            }
        }

        [JsonIgnore]
        public Product SelectedProduct
        {
            get
            {
                if (SelectedGroup > -1 && SelectedGroup < ProductGroups.Count) return ProductGroups[SelectedGroup].SelectedProduct;

                return null;
            }
        }

        #endregion

        #region Public Commands

        [JsonIgnore]
        public ICommand UndoCommand { get; set; }

        [JsonIgnore]
        public ICommand RedoCommand { get; set; }

        [JsonIgnore]
        public ICommand AddTabCommand { get; set; }

        [JsonIgnore]
        public ICommand DeleteTabCommand { get; set; }

        [JsonIgnore]
        public List<ICommand> AddProductCommands { get; set; }

        [JsonIgnore]
        public List<ICommand> DeleteProductCommands { get; set; }

        [JsonIgnore]
        public List<ICommand> AddProductPropertyCommands { get; set; }

        [JsonIgnore]
        public ICommand EditProductPropertyCommand { get; set; }

        [JsonIgnore]
        public List<ICommand> DeleteProductPropertyCommands { get; set; }

        #endregion

        #region Constructors

        public ProductPageViewModel()
        {
            ProductGroups = new ObservableCollection<ProductGroup>();
            trackService = new TrackService();
            trackService.PropertyChanged += OnPropertyChanged;
            SelectedGroup = -1;

            UndoCommand = trackService.GetUndoCommand();
            RedoCommand = trackService.GetRedoCommand();
            AddTabCommand = new TrackableDelegateCommand<ProductGroup>(AddTab_Execute, AddTab_CanExecute, AddTab_Undo, AddTab_Redo, "AddTabCommand", trackService);
            DeleteTabCommand = new TrackableDelegateCommand<ProductGroup>(DeleteTab_Execute, DeleteTab_CanExecute, DeleteTab_Undo, DeleteTab_Redo, "DeleteTabCommand", trackService);

            AddProductCommands = new List<ICommand>();
            DeleteProductCommands = new List<ICommand>();
            AddProductPropertyCommands = new List<ICommand>();
            DeleteProductPropertyCommands = new List<ICommand>();

            CommandService.LoadCommand.RegisterCommand(new RelayCommand(() => Load_Execute()));
            CommandService.SaveCommand.RegisterCommand(new RelayCommand(() => Save_Execute()));
        }

        #endregion

        #region Command Methods

        #region AddTab Command

        private ProductGroup AddTab_Execute()
        {
            ProductGroup group = new ProductGroup("New Group", trackService);
            ProductGroups.Add(group);
            AddProductCommands.Add(new TrackableDelegateCommand<Product>(AddProduct_Execute, AddProduct_CanExecute, AddProduct_Undo, AddProduct_Redo, "AddProductCommand", group.ItemTrackService));
            DeleteProductCommands.Add(new TrackableDelegateCommand<Product, Product>(DeleteProduct_Execute, DeleteProduct_CanExecute, DeleteProduct_Undo, DeleteProduct_Redo, "DeleteProductCommand", group.ItemTrackService));
            AddProductPropertyCommands.Add(new TrackableDelegateCommand<Property>(AddProductProperty_Execute, AddProductProperty_CanExecute, AddProductProperty_Undo, AddProductProperty_Redo, "AddProductPropertyCommand", group.ItemTrackService));
            DeleteProductPropertyCommands.Add(new TrackableDelegateCommand<Property, Property>(DeleteProductProperty_Execute, DeleteProductProperty_CanExecute, DeleteProductProperty_Undo, DeleteProductProperty_Redo, "DeleteProductPropertyCommand", group.ItemTrackService));
            group.Index = SelectedGroup = ProductGroups.Count - 1;

            group.ItemTrackService.PropertyChanged += OnMemberPropertyChanged;

            return group;
        }

        private bool AddTab_CanExecute()
        {
            return true;
        }

        private ProductGroup AddTab_Undo(ProductGroup group)
        {
            group.Index = ProductGroups.IndexOf(group);
            ProductGroups.Remove(group);
            AddProductCommands.RemoveAt(group.Index);
            DeleteProductCommands.RemoveAt(group.Index);
            AddProductPropertyCommands.RemoveAt(group.Index);
            DeleteProductPropertyCommands.RemoveAt(group.Index);

            group.ItemTrackService.PropertyChanged -= OnMemberPropertyChanged;

            return group;
        }

        private ProductGroup AddTab_Redo(ProductGroup group)
        {
            AddProductCommands.Insert(group.Index, new TrackableDelegateCommand<Product>(AddProduct_Execute, AddProduct_CanExecute, AddProduct_Undo, AddProduct_Redo, "AddProductCommand", group.ItemTrackService));
            DeleteProductCommands.Add(new TrackableDelegateCommand<Product, Product>(DeleteProduct_Execute, DeleteProduct_CanExecute, DeleteProduct_Undo, DeleteProduct_Redo, "DeleteProductCommand", group.ItemTrackService));
            AddProductPropertyCommands.Add(new TrackableDelegateCommand<Property>(AddProductProperty_Execute, AddProductProperty_CanExecute, AddProductProperty_Undo, AddProductProperty_Redo, "AddProductPropertyCommand", group.ItemTrackService));
            DeleteProductPropertyCommands.Add(new TrackableDelegateCommand<Property, Property>(DeleteProductProperty_Execute, DeleteProductProperty_CanExecute, DeleteProductProperty_Undo, DeleteProductProperty_Redo, "DeleteProductPropertyCommand", group.ItemTrackService));
            ProductGroups.Insert(group.Index, group);

            group.ItemTrackService.PropertyChanged += OnMemberPropertyChanged;

            return group;
        }

        #endregion

        #region DeleteTab Command

        private ProductGroup DeleteTab_Execute()
        {
            ProductGroup group = ProductGroups[SelectedGroup];
            group.Index = SelectedGroup;
            ProductGroups.Remove(group);
            AddProductCommands.RemoveAt(group.Index);
            DeleteProductCommands.RemoveAt(group.Index);
            AddProductPropertyCommands.RemoveAt(group.Index);
            DeleteProductPropertyCommands.RemoveAt(group.Index);

            group.ItemTrackService.PropertyChanged -= OnMemberPropertyChanged;

            return group;
        }

        private bool DeleteTab_CanExecute()
        {
            if (SelectedGroup >= 0 && SelectedGroup < ProductGroups.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private ProductGroup DeleteTab_Undo(ProductGroup group)
        {
            AddProductCommands.Insert(group.Index, new TrackableDelegateCommand<Product>(AddProduct_Execute, AddProduct_CanExecute, AddProduct_Undo, AddProduct_Redo, "AddProductCommand", group.ItemTrackService));
            DeleteProductCommands.Add(new TrackableDelegateCommand<Product, Product>(DeleteProduct_Execute, DeleteProduct_CanExecute, DeleteProduct_Undo, DeleteProduct_Redo, "DeleteProductCommand", group.ItemTrackService));
            AddProductPropertyCommands.Add(new TrackableDelegateCommand<Property>(AddProductProperty_Execute, AddProductProperty_CanExecute, AddProductProperty_Undo, AddProductProperty_Redo, "AddProductPropertyCommand", group.ItemTrackService));
            DeleteProductPropertyCommands.Add(new TrackableDelegateCommand<Property, Property>(DeleteProductProperty_Execute, DeleteProductProperty_CanExecute, DeleteProductProperty_Undo, DeleteProductProperty_Redo, "DeleteProductPropertyCommand", group.ItemTrackService));
            ProductGroups.Insert(group.Index, group);
            SelectedGroup = group.Index;

            group.ItemTrackService.PropertyChanged += OnMemberPropertyChanged;

            return group;
        }

        private ProductGroup DeleteTab_Redo(ProductGroup group)
        {
            ProductGroups.Remove(group);
            AddProductCommands.RemoveAt(group.Index);
            DeleteProductCommands.RemoveAt(group.Index);
            AddProductPropertyCommands.RemoveAt(group.Index);
            DeleteProductPropertyCommands.RemoveAt(group.Index);

            group.ItemTrackService.PropertyChanged -= OnMemberPropertyChanged;

            return group;
        }

        #endregion

        #region AddProduct Command

        private Product AddProduct_Execute()
        {
            Product product = new Product(ProductGroups[selectedGroup].ItemTrackService);
            ProductGroups[selectedGroup].Products.Add(product);
            product.Index = ProductGroups[selectedGroup].Products.Count - 1;
            return product;
        }

        private bool AddProduct_CanExecute()
        {
            return selectedGroup > -1 ? true : false;
        }

        private Product AddProduct_Undo(Product product)
        {
            ProductGroups[SelectedGroup].Products.Remove(product);
            return product;
        }

        private Product AddProduct_Redo(Product product)
        {
            ProductGroups[selectedGroup].Products.Insert(product.Index, product);
            return product;
        }

        #endregion

        #region DeleteTabCommand

        private Product DeleteProduct_Execute(Product product)
        {
            product.Index = ProductGroups[SelectedGroup].Products.IndexOf(product);
            ProductGroups[SelectedGroup].Products.Remove(product);
            return product;
        }

        private bool DeleteProduct_CanExecute(Product product)
        {
            if (product != null && selectedGroup >= 0 && selectedGroup < ProductGroups.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Product DeleteProduct_Undo(Product product)
        {
            ProductGroups[selectedGroup].Products.Insert(product.Index, product);
            return product;
        }

        private Product DeleteProduct_Redo(Product product)
        {
            ProductGroups[SelectedGroup].Products.Remove(product);
            return product;
        }

        #endregion

        #region AddProductProperty Command

        private Property AddProductProperty_Execute()
        {
            Property property = new Property(ProductGroups[selectedGroup].ItemTrackService);
            ProductGroups[SelectedGroup].SelectedProduct.Properties.Add(property);
            property.Index = ProductGroups[SelectedGroup].SelectedProduct.Properties.Count - 1;
            return property;
        }

        private bool AddProductProperty_CanExecute()
        {
            if (SelectedGroup > -1 && ProductGroups[SelectedGroup].SelectedProduct != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Property AddProductProperty_Undo(Property property)
        {
            ProductGroups[SelectedGroup].SelectedProduct.Properties.Remove(property);
            property.Index = ProductGroups[SelectedGroup].SelectedProduct.Properties.Count - 1;
            return property;
        }

        private Property AddProductProperty_Redo(Property property)
        {
            ProductGroups[SelectedGroup].SelectedProduct.Properties.Insert(property.Index, property);
            return property;
        }

        #endregion

        #region DeleteProductProperty Command

        private Property DeleteProductProperty_Execute(Property property)
        {
            property.Index = ProductGroups[SelectedGroup].SelectedProduct.Properties.IndexOf(property);
            ProductGroups[SelectedGroup].SelectedProduct.Properties.Remove(property);
            return property;
        }

        private bool DeleteProductProperty_CanExecute(Property property)
        {
            if (SelectedGroup > -1 && ProductGroups[SelectedGroup].SelectedProduct != null && property != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Property DeleteProductProperty_Undo(Property property)
        {
            ProductGroups[SelectedGroup].SelectedProduct.Properties.Insert(property.Index, property);
            return property;
        }

        private Property DeleteProductProperty_Redo(Property property)
        {
            property.Index = ProductGroups[SelectedGroup].SelectedProduct.Properties.IndexOf(property);
            ProductGroups[SelectedGroup].SelectedProduct.Properties.Remove(property);
            return property;
        }

        #endregion

        #region Load Command

        private void Load_Execute()
        {
            string filePath = "./data" + "_products" + ".json";

            if (File.Exists(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();

                using (var sw = new StreamReader(filePath))
                using (var reader = new JsonTextReader(sw))
                {
                    serializer.Converters.Add(new ProductViewModelJsonConverter(trackService));
                    ProductGroups = serializer.Deserialize<ObservableCollection<ProductGroup>>(reader);
                }
            }

            for (int i = 0; i < ProductGroups.Count; ++i)
            {
                AddProductCommands.Add(new TrackableDelegateCommand<Product>(AddProduct_Execute, AddProduct_CanExecute, AddProduct_Undo, AddProduct_Redo, "AddProductCommand", ProductGroups[i].ItemTrackService));
                DeleteProductCommands.Add(new TrackableDelegateCommand<Product, Product>(DeleteProduct_Execute, DeleteProduct_CanExecute, DeleteProduct_Undo, DeleteProduct_Redo, "DeleteProductCommand", ProductGroups[i].ItemTrackService));
            }
        }

        #endregion

        #region Save Command

        private void Save_Execute()
        {
            string filePath = "./data" + "_products" + ".json";

            JsonSerializer serializer = new JsonSerializer();
            using (var sw = new StreamWriter(filePath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, this);
            }
        }

        #endregion

        #endregion

        #region Event Handlers

        /// <summary>
        /// Called every time a product group fires an OnPropertyChanged event.
        /// If the event arguments property name is appropriate we fire an OnPropertyChanged event for the calculated property.
        /// </summary>
        /// <param name="sender">The sender of the OnPropertyChanged event.</param>
        /// <param name="args">The arguments of the OnPropertyChanged event.</param>
        /// TODO: This looks a little bit sketchy!!!
        public void OnMemberPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Equals("SelectedProduct"))
            {
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        #endregion
    }
}
