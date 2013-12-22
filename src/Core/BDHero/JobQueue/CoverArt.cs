using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DotNetUtils.Annotations;
using DotNetUtils.Net;
using I18N;

namespace BDHero.JobQueue
{
    /// <summary>
    /// Represents a movie or TV show's official cover art (a.k.a. poster) image.
    /// </summary>
    public interface ICoverArt
    {
        /// <summary>
        /// Gets or sets the language of the cover art image.
        /// </summary>
        Language Language { get; set; }

        /// <summary>
        /// Gets or sets whether this CoverArt will be used in the output file.
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// Gets the poster image.
        /// </summary>
        Image Image { get; }
    }

    /// <summary>
    /// Remote web-based cover art, loaded synchronously over HTTP.
    /// </summary>
    public class RemoteCoverArt : ICoverArt
    {
        /// <summary>
        /// Gets or sets the URI of the cover art image.
        /// </summary>
        public string Uri;

        public Language Language { get; set; }

        public bool IsSelected { get; set; }

        /// <summary>
        /// Retrieves the image for the cover art by making an HTTP GET request for the <see cref="Uri"/>.
        /// </summary>
        /// <remarks>
        /// <strong>NOTE:</strong> The first time this property is accessed it will BLOCK until the HTTP request completes or throws an exception.
        /// Subsequent requests will immediately return a cached copy of the image and will not block.
        /// UIs should access this property from a separate thread if possible to avoid freezing the UI on the first request.
        /// </remarks>
        public Image Image
        {
            get { return HttpRequest.GetImage(Uri); }
        }
    }

    /// <summary>
    /// Local in-memory cover art.
    /// </summary>
    public class InMemoryCoverArt : ICoverArt
    {
        public Language Language { get; set; }

        public bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets the poster image.
        /// </summary>
        public Image Image { get; set; }
    }
}
