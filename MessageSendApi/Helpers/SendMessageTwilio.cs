using System;
using Twilio.Rest.Api.V2010.Account;
using Twilio;
using DotNetEnv;
using MessageSendApi.Models;

namespace MessageSendApi.Helpers;

public abstract class SendMessageTwilio
{
    public static async Task<MessageResource> sendMessage(Message message)
    {
        var AccountSid = Env.GetString("ACCOUNT_SID");
        var AuthToken = Env.GetString("AUTH_TOKEN");
        var phoneFrom = Env.GetString("PHONE_NUMBER");

        TwilioClient.Init(AccountSid, AuthToken);

        var messageTwilio = await MessageResource.CreateAsync(
            body: message.MessageBody,
            from: new Twilio.Types.PhoneNumber(phoneFrom),
            to: new Twilio.Types.PhoneNumber(message.To)
        );

        return messageTwilio;
    }
}
