﻿using System;
using System.Collections.Generic;
using FullSerializer;
using Models;
using Proyecto26;

public static class DatabaseHandler
{
    private const string projectId = "kohoisland-cef6e"; // You can find this in your Firebase project settings
    private static readonly string databaseURL = $"https://{projectId}.firebaseio.com/";
    
    private static fsSerializer serializer = new fsSerializer();

    public delegate void PostUserCallback();
    public delegate void GetUserCallback(playerScore user);
    public delegate void GetUsersCallback(Dictionary<string, playerScore> users);
    
    
    /// <summary>
    /// Adds a user to the Firebase Database
    /// </summary>
    /// <param name="user"> User object that will be uploaded </param>
    /// <param name="userId"> Id of the user that will be uploaded </param>
    /// <param name="callback"> What to do after the user is uploaded successfully </param>
    public static void PostUser(playerScore user, string userId, PostUserCallback callback)
    {
        RestClient.Put<playerScore>($"{databaseURL}users/{userId}.json", user).Then(response => { callback(); });
    }

    /// <summary>
    /// Retrieves a user from the Firebase Database, given their id
    /// </summary>
    /// <param name="userId"> Id of the user that we are looking for </param>
    /// <param name="callback"> What to do after the user is downloaded successfully </param>
    public static void GetUser(string userId, GetUserCallback callback)
    {
        RestClient.Get<playerScore>($"{databaseURL}users/{userId}.json").Then(user => { callback(user); });
    }

    /// <summary>
    /// Gets all users from the Firebase Database
    /// </summary>
    /// <param name="callback"> What to do after all users are downloaded successfully </param>
    public static void GetUsers(GetUsersCallback callback)
    {
        RestClient.Get($"{databaseURL}users.json").Then(response =>
        {
            var responseJson = response.Text;

            // Using the FullSerializer library: https://github.com/jacobdufault/fullserializer
            // to serialize more complex types (a Dictionary, in this case)
            var data = fsJsonParser.Parse(responseJson);
            object deserialized = null;
            serializer.TryDeserialize(data, typeof(Dictionary<string, playerScore>), ref deserialized);

            var users = deserialized as Dictionary<string, playerScore>;
            callback(users);
        });
    }
    
    
    
}