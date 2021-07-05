using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PDADesktop
{
    public class TablePageViewModel : BaseViewModel
    {
        #region Private Members

        private int maxTables;
        private Table selectedTable;

        #endregion

        #region Public Properties

        public int MaxTables
        {
            get
            {
                return maxTables;
            }

            private set
            {
                if (maxTables != value)
                {
                    maxTables = value;
                    OnPropertyChanged(nameof(MaxTables));
                    OnPropertyChanged(nameof(CurrentTableCount));
                }
            }
        }

        public Table SelectedTable
        {
            get
            {
                return selectedTable;
            }

            set
            {
                if (selectedTable != value)
                {
                    selectedTable = value;
                    OnPropertyChanged(nameof(SelectedTable));
                }
            }
        }

        public ObservableCollection<Table> Tables { get; set; }

        public string CurrentTableCount
        {
            get
            {
                return Tables.Count + " / " + maxTables;
            }
        }

        #endregion

        #region Public Commands

        public ICommand AddTableCommand { get; set; }

        public ICommand RemoveTableCommand { get; set; }

        public ICommand UndoCommand { get; set; }

        public ICommand RedoCommand { get; set; }

        #endregion

        #region Constructors

        public TablePageViewModel()
        {
            trackService = new TrackService();
            MaxTables = 5;
            Tables = new ObservableCollection<Table>();
            Tables.Add(new Table());
            Tables.Add(new Table());

            AddTableCommand = new TrackableDelegateCommand<Table>(AddTable_Execute, AddTable_CanExecute, AddTable_Undo, AddTable_Redo, "AddTableCommand", trackService);
            RemoveTableCommand = new TrackableDelegateCommand<Table, Table>(RemoveTable_Execute, RemoveTable_CanExecute, RemoveTable_Undo, RemoveTable_Redo, "RemoveTableCommand", trackService);
            UndoCommand = trackService.GetUndoCommand();
            RedoCommand = trackService.GetRedoCommand();
        }

        #endregion

        #region AddTable Command

        private Table AddTable_Execute()
        {
            Table table = new Table();
            Tables.Add(table);
            OnPropertyChanged(nameof(CurrentTableCount));
            return table;
        }

        private bool AddTable_CanExecute()
        {
            if (Tables.Count < MaxTables) return true;

            return false;
        }

        private Table AddTable_Undo(Table table)
        {
            Tables.Remove(table);
            OnPropertyChanged(nameof(CurrentTableCount));
            return table;
        }

        private Table AddTable_Redo(Table table)
        {
            Tables.Add(table);
            OnPropertyChanged(nameof(CurrentTableCount));
            return table;
        }

        #endregion

        #region RemoveTable Command

        private Table RemoveTable_Execute(Table table)
        {
            Tables.Remove(table);
            OnPropertyChanged(nameof(CurrentTableCount));
            return table;
        }

        private bool RemoveTable_CanExecute(Table table)
        {
            if (table != null && Tables.Count > 0) return true;

            return false;
        }

        private Table RemoveTable_Undo(Table table)
        {
            Tables.Add(table);
            OnPropertyChanged(nameof(CurrentTableCount));
            return table;
        }

        private Table RemoveTable_Redo(Table table)
        {
            Tables.Remove(table);
            OnPropertyChanged(nameof(CurrentTableCount));
            return table;
        }

        #endregion
    }
}
