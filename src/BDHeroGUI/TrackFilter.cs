using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using BDHero.BDROM;
using I18N;

namespace BDHeroGUI
{
    public class TrackFilter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Language _preferredLanguage = Language.CurrentUILanguage;
        public Language PreferredLanguage
        {
            get { return _preferredLanguage; }
            set { SetField(ref _preferredLanguage, value, () => PreferredLanguage); }
        }

        private IList<TrackType> _trackTypes = new List<TrackType> { TrackType.MainFeature };
        public IList<TrackType> TrackTypes
        {
            get { return _trackTypes; }
            set { SetField(ref _trackTypes, value, () => TrackTypes); }
        }

        private bool _hideHiddenTracks = true;
        public bool HideHiddenTracks
        {
            get { return _hideHiddenTracks; }
            set { SetField(ref _hideHiddenTracks, value, () => HideHiddenTracks); }
        }

        private bool _hideUnsupportedCodecs = true;
        public bool HideUnsupportedCodecs
        {
            get { return _hideUnsupportedCodecs; }
            set { SetField(ref _hideUnsupportedCodecs, value, () => HideUnsupportedCodecs); }
        }

        public bool Show(Track track)
        {
            var show = track.Language == PreferredLanguage &&
                       TrackTypes.Contains(track.Type);
            var hide = (track.IsHidden && HideHiddenTracks) ||
                       (!track.Codec.IsMuxable && HideUnsupportedCodecs);
            return show && !hide;
        }

        [Annotations.NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> selectorExpression)
        {
            if (selectorExpression == null)
                throw new ArgumentNullException("selectorExpression");
            MemberExpression body = selectorExpression.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("The body must be a member expression");
            OnPropertyChanged(body.Member.Name);
        }

        protected bool SetField<T>(ref T field, T value, Expression<Func<T>> selectorExpression)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(selectorExpression);
            return true;
        }
    }
}
