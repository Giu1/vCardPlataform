using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace vCardPlatformAPI.Models
{
    
    public class Mosquito
    {
        
         MqttClient broker = new MqttClient("127.0.0.1");
         string[] topics = { "#1", "#2" };
        static byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };
        
        public void Conn()
        {
            try
            {
                if (!broker.IsConnected)
                {
                    broker.Connect(Guid.NewGuid().ToString());
                    if (!broker.IsConnected)
                    {
                        return;// "Erro1";
                    }
                }
                

            }
            catch (Exception)
            {
                return;// "Erro2";
            }
            broker.Subscribe(topics, qosLevels);

            Console.WriteLine("S");
        }

        public void Publish(string content)
        {
            try
            {
                if (broker.IsConnected)
                {
                    byte[] msg = Encoding.UTF8.GetBytes(content);
                    broker.Publish(topics[1], msg);
                }
                else
                {
                    broker.Connect(Guid.NewGuid().ToString());
                    byte[] msg = Encoding.UTF8.GetBytes("What");
                    broker.Publish(topics[1], msg);
                    Console.WriteLine("S");

                    if (broker.IsConnected)
                    {
                        broker.Unsubscribe(topics); //Put this in a button to see notif!
                        broker.Disconnect(); //Free process and process's resources
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;// "Erro2";
            }

           

        }
        public void Close()
        {
            try
            {
                if (broker.IsConnected)
                {
                    broker.Unsubscribe(topics); //Put this in a button to see notify!
                    broker.Disconnect(); //Free process and process's resources
                    return;
                }
                return;
            }
            catch (Exception)
            {
                return;// "Erro2";
            }
        }
    }
}