using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Boggle
{
    [ServiceContract]
    public interface IBoggleService
    {
        /// <summary>
        /// Sends back index.html as the response body.
        /// </summary>
        [WebGet(UriTemplate = "/api")]
        Stream API();

        /// <summary>
        /// Registers a new user using the provided nickname.
        /// Returns the user's token.
        /// </summary>
        [WebInvoke(Method = "POST", UriTemplate = "/users")]
        UserID Register(RegisterRequest Nickname);

        /// <summary>
        /// Joins a game.
        /// Returns game ID.
        /// </summary>
        [WebInvoke(Method = "POST", UriTemplate = "/games")]
        gameID Join(JoinRequest Request);

        /// <summary>
        /// Cancels join request.
        /// </summary>
        [WebInvoke(Method = "PUT", UriTemplate = "/games")]
        void Cancel(UserID UserToken);

        /// <summary>
        /// Cancels join request.
        /// Returns score.
        /// </summary>
        [WebInvoke(Method = "PUT", UriTemplate = "/games/{GameID}")]
        score Play(PlayWord Word, string GameID);

        /// <summary>
        /// Cancels join request.
        /// Returns score.
        /// </summary>
        [WebGet(UriTemplate = "/games/{GameID}/{Brief=No}")]
        Game Status(string GameID, string Brief);
    }
}
