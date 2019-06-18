using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Linq;

namespace SilverlightApplication2
{
    public class Customer : INotifyDataErrorInfo, INotifyPropertyChanged
    {
        Dictionary<string, List<string>> propErrors = new Dictionary<string, List<string>>();

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private void Validate()
        {
            List<string> listErrors;
            if (propErrors.TryGetValue(Name, out listErrors) == false)
                listErrors = new List<string>();
            else
                listErrors.Clear();

            if (string.IsNullOrEmpty(Name))
                listErrors.Add("Name should not be empty!!!");
            else if (string.Equals(Name, "Srini"))
                listErrors.Add("Enter a different name!!!");
            propErrors["Name"] = listErrors;

            if (listErrors.Count > 0)
            {
                OnPropertyErrorsChanged("Name");
            }
        }

        public bool HasErrors
        {
            get
            {
                try
                {
                    var propErrorsCount = propErrors.Values.FirstOrDefault(r => r.Count > 0);
                    if (propErrorsCount != null)
                        return true;
                    else
                        return false;
                }
                catch { }
                return true;
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void OnPropertyErrorsChanged(string p)
        {
            if (ErrorsChanged != null)
                ErrorsChanged.Invoke(this, new DataErrorsChangedEventArgs(p));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            List<string> errors = new List<string>();
            if (propertyName != null)
            {
                propErrors.TryGetValue(propertyName, out errors);
                return errors;
            }
            else
                return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                Validate();
            }
        }
    }
}
