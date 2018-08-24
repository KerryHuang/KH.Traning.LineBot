using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebHook_First.Enum
{
    /// <summary>
    /// JSON objects which contain events generated on the LINE Platform.
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// Webhook event object which contains the sent message. The message property contains a message object which corresponds with the message type. You can reply to message events.
        /// </summary>
        Message,

        /// <summary>
        /// Event object for when your account is added as a friend (or unblocked). You can reply to follow events.
        /// </summary>
        Follow,

        /// <summary>
        /// Event object for when your account is blocked.
        /// </summary>
        Unfollow,

        /// <summary>
        /// Event object for when your account joins a group or talk room. You can reply to join events.
        /// </summary>
        Join,

        /// <summary>
        /// Event object for when a user deletes your bot from a group.
        /// No leave event is generated in the following situations:
        ///     1. A user deletes your bot from a room
        ///     2. Bot leaves a group or room using a request to the leave group or leave room endpoints
        /// </summary>
        Leave,

        /// <summary>
        /// Event object for when a user performs an action on a template message which initiates a postback. You can reply to postback events.
        /// </summary>
        Postback,

        /// <summary>
        /// Event object for when a user enters or leaves the range of a LINE Beacon. You can reply to beacon events.
        /// </summary>
        Beacon,

        /// <summary>
        /// Event object for when a user has linked his/her LINE account with a provider's service account. You can reply to account link events.
        /// If the link token has expired or has already been used, no webhook event will be sent and the user will be shown an error.
        /// </summary>
        AccountLink
    }
}