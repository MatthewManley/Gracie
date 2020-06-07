using Gracie.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gracie.Http
{
    /// <summary>
    /// https://discord.com/developers/docs/resources/user#users-resource
    /// </summary>
    public class UserApi
    {
        /// <summary>
        /// Returns the user object of the requester's account.
        /// For OAuth2, this requires the identify scope, which will return the object without an email, and optionally the email scope, which returns the object with an email.
        /// https://discord.com/developers/docs/resources/user#get-current-user
        /// </summary>
        public User GetCurrentUser()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a user object for a given user ID.
        /// https://discord.com/developers/docs/resources/user#get-user
        /// </summary>
        public User GetUser(ulong id)
        {
            throw new NotImplementedException();
        }

        //TODO: Modify Current User https://discord.com/developers/docs/resources/user#modify-current-user

        /// <summary>
        /// Returns a list of partial guild objects the current user is a member of.
        /// Requires the guilds OAuth2 scope.
        /// https://discord.com/developers/docs/resources/user#get-current-user-guilds
        /// </summary>
        public List<Guild> GetCurrentUserGuilds()
        {
            throw new NotImplementedException();
        }

        //TODO: Leave Guild https://discord.com/developers/docs/resources/user#leave-guild
        //TODO: Get User DMs https://discord.com/developers/docs/resources/user#get-user-dms
        //TODO: Create DM https://discord.com/developers/docs/resources/user#create-dm
        //TODO: Create Group DM https://discord.com/developers/docs/resources/user#create-group-dm
        //TODO: Get User Connections https://discord.com/developers/docs/resources/user#get-user-connections
    }
}
