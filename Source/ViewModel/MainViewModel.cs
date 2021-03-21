using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using RollingLineSavegameFix.Model;

namespace RollingLineSavegameFix.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        MainModel _model;

        public MainViewModel()
        {
            _model = new MainModel();
        }


        public string FileName
        {
            get => _model.FileName;
            set
            {
                if (_model.FileName == value)
                    return;

                _model.FileName = value;
                LoadFileContent();
                OnPropertyChanged(nameof(FileName));
            }
        }

        /// <summary>
        ///     Remove all Waggons
        /// </summary>
        public bool ShouldRemoveAllWaggons
        {
            get => _model.ShouldRemoveAllWaggons; set
            {
                if (_model.ShouldRemoveAllWaggons == value)
                    return;

                _model.ShouldRemoveAllWaggons = value;
                OnPropertyChanged(nameof(ShouldRemoveAllWaggons));
            }
        }

        /// <summary>
        ///     Remove faulty Waggons
        /// </summary>
        public bool ShouldRemoveFaultyWaggons
        {
            get => _model.ShouldRemoveFaultyWaggons; set
            {
                if (_model.ShouldRemoveFaultyWaggons == value)
                    return;

                _model.ShouldRemoveFaultyWaggons = value;
                OnPropertyChanged(nameof(ShouldRemoveFaultyWaggons));
            }
        }

        /// <summary>
        /// Removes all Breaks
        /// </summary>
        public bool ShouldRemoveBreaks
        {
            get => _model.ShouldRemoveBreaks; set
            {
                if (_model.ShouldRemoveBreaks == value)
                    return;

                _model.ShouldRemoveBreaks = value;
                OnPropertyChanged(nameof(ShouldRemoveBreaks));
            }
        }


        private void LoadFileContent()
        {
            try
            {
                _model.FileContent = File.ReadAllText(_model.FileName);
            }
            catch (IOException)
            {
                MessageBox.Show("Error reading file: " + _model.FileName, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _model.FileContent = null;
                _model.FileName = null;
            }
            OnPropertyChanged(nameof(AreOptionsAvaiable));
            OnPropertyChanged(nameof(FileName));

        }
        
        public bool AreOptionsAvaiable
        {
            get => !string.IsNullOrWhiteSpace(FileName);            
        }
    }
}
