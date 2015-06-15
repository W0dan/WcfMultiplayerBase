﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiplayerServer
{
    public class SubscriberCollection<TSubscriberCallback>
    {
        private readonly List<Subscriber<TSubscriberCallback>> _subscribers = new List<Subscriber<TSubscriberCallback>>();

        public Subscriber<TSubscriberCallback> Add(string name, TSubscriberCallback callback)
        {
            try
            {
                if (_subscribers.Any(s => s.Name == name))
                {
                    return null;
                }

                var subscriber = new Subscriber<TSubscriberCallback>
                {
                    Token = Guid.NewGuid().ToString(),
                    Name = name,
                    Callback = callback
                };
                _subscribers.Add(subscriber);
                return subscriber;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void Send(string token, Action<string, TSubscriberCallback> action)
        {
            Task.Factory.StartNew(() =>
            {
                var subscriberFrom = _subscribers.Single(s => s.Token == token);

                var subscribersToRemove = new List<Subscriber<TSubscriberCallback>>();

                foreach (var subscriber in _subscribers)
                {
                    try
                    {
                        action(subscriberFrom.Name, subscriber.Callback);
                    }
                    catch
                    {
                        subscribersToRemove.Add(subscriber);
                    }
                }

                foreach (var subscriber in subscribersToRemove)
                {
                    _subscribers.Remove(subscriber);
                }
            });
        }
    }
}