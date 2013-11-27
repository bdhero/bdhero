using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using BDHero.BDROM;
using DotNetUtils.Annotations;

namespace BDHeroGUI
{
    public class PlaylistFilter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private TimeSpan _minDuration = TimeSpan.FromMinutes(5);
        public TimeSpan MinDuration
        {
            get { return _minDuration; }
            set { SetField(ref _minDuration, value, () => MinDuration); }
        }

        private int _minChapterCount = 2;
        public int MinChapterCount
        {
            get { return _minChapterCount; }
            set { SetField(ref _minChapterCount, value, () => MinChapterCount); }
        }

        private IList<TrackType> _trackTypes = new List<TrackType> { TrackType.MainFeature };
        public IList<TrackType> TrackTypes
        {
            get { return _trackTypes; }
            set { SetField(ref _trackTypes, value, () => TrackTypes); }
        }

        private bool _hideDuplicatePlaylists = true;
        public bool HideDuplicatePlaylists
        {
            get { return _hideDuplicatePlaylists; }
            set { SetField(ref _hideDuplicatePlaylists, value, () => HideDuplicatePlaylists); }
        }

        private bool _hideDuplicateStreamClips = true;
        public bool HideDuplicateStreamClips
        {
            get { return _hideDuplicateStreamClips; }
            set { SetField(ref _hideDuplicateStreamClips, value, () => HideDuplicateStreamClips); }
        }

        private bool _hideLoops = true;
        public bool HideLoops
        {
            get { return _hideLoops; }
            set { SetField(ref _hideLoops, value, () => HideLoops); }
        }

        private bool _hideHiddenFirstTracks = true;
        public bool HideHiddenFirstTracks
        {
            get { return _hideHiddenFirstTracks; }
            set { SetField(ref _hideHiddenFirstTracks, value, () => HideHiddenFirstTracks); }
        }

        public bool Show(Playlist playlist)
        {
            var show = playlist.Length >= MinDuration &&
                       playlist.ChapterCount >= MinChapterCount &&
                       TrackTypes.Contains(playlist.Type);
            var hide = (playlist.IsDuplicate && HideDuplicatePlaylists) ||
                       (playlist.HasDuplicateStreamClips && HideDuplicateStreamClips) ||
                       (playlist.HasLoops && HideLoops) ||
                       (playlist.HasHiddenFirstTracks && HideHiddenFirstTracks);
            return show && !hide;
        }

        [NotifyPropertyChangedInvocator]
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
