using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tx.ToolBox.Helpers;

namespace Tx.ToolBox.Settings
{
    public class ObservableSettings<TSettings> : ISettingsContainer, IObservable<TSettings>
        where TSettings : class, new()
    {
        public ObservableSettings(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Null, empty string or whitpaces cannot be used as Id");
            Id = id;
        }

        public ObservableSettings(TSettings settings, string id) : this(id)
        {
            Settings = settings;
        }

        public string Id { get; }
        public TSettings Settings { get; private set; }
        object ISettingsContainer.Settings => Settings;
        public Type SettingsType => typeof(TSettings);

        public void Set(object settings)
        {
            Settings = (TSettings)settings;
            lock (_observers)
            {
                _observers.ForEach(o => o.OnNext(Settings));
            }
        }

        public IDisposable Subscribe(IObserver<TSettings> observer)
        {
            observer.OnNext(Settings);
            lock (_observers)
            {
                _observers.Add(observer);
                return new Action(() =>
                {
                    lock (_observers)
                    {
                        _observers.Remove(observer);
                    }
                }).AsDisposable();
            }
        }

        public void CopyTo(ISettingsStorage otherStorage)
        {
            otherStorage.SetSettings(Settings, Id);
        }

        public void Dispose()
        {
            lock (_observers)
            {
                _observers.ForEach(o => o.OnCompleted());
            }
        }

        private readonly List<IObserver<TSettings>> _observers = new List<IObserver<TSettings>>();
    }
}
