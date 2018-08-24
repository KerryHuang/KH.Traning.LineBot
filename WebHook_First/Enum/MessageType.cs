using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebHook_First.Enum
{
    /// <summary>
    /// Object containing the contents of the message. Message types include:
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Message object which contains the text sent from the source.
        /// </summary>
        Text,

        /// <summary>
        /// Message object which contains the image content sent from the source. The binary image data can be retrieved from the content endpoint.
        /// </summary>
        Image,

        /// <summary>
        /// Message object which contains the video content sent from the source. The binary video data can be retrieved from the content endpoint.
        /// </summary>
        Video,

        /// <summary>
        /// Message object which contains the audio content sent from the source. The binary audio data can be retrieved from the content endpoint.
        /// </summary>
        Audio,

        /// <summary>
        /// Message object which contains the file sent from the source. The binary data can be retrieved from the content endpoint.
        /// </summary>
        File,

        /// <summary>
        /// Message object which contains the location data sent from the source.
        /// </summary>
        Location,

        /// <summary>
        /// Message object which contains the sticker data sent from the source. For a list of basic LINE stickers and sticker IDs, see sticker list.
        /// https://developers.line.me/media/messaging-api/sticker_list.pdf
        /// </summary>
        Sticker
    }
}