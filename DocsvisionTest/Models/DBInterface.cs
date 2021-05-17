using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace DocsvisionTest.Models
{
    public class DBInterface
    {
        private readonly IConfiguration Configuration;
        public DBInterface (IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        //Получение всех писем из БД
        public IEnumerable<Message> GetMessageList()
        {
            List<Message> messages = new List<Message>();
            using(SqlConnection connection = GetSqlConnection())
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Messages", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        messages.Add(new Message
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Title = reader["Title"].ToString(),
                            DateTime = Convert.ToDateTime(reader["Datetime"]),
                            Recipient = reader["Recipient"].ToString(),
                            SenderID = Convert.ToInt32(reader["Sender"]),
                            Sender = GetSender(Convert.ToInt32(reader["Sender"])),
                            Content = reader["Content"].ToString(),
                            Important = Convert.ToBoolean(reader["Important"]),
                            Tags = GetTags(Convert.ToInt32(reader["ID"]))
                        }) ;
                    }
                }
            }
            return messages;
        }

        public IEnumerable<Message> GetMessageList(string sql_command)
        {
            List<Message> messages = new List<Message>();
            using (SqlConnection connection = GetSqlConnection())
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql_command, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        messages.Add(new Message
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Title = reader["Title"].ToString(),
                            DateTime = Convert.ToDateTime(reader["Datetime"]),
                            Recipient = reader["Recipient"].ToString(),
                            SenderID = Convert.ToInt32(reader["Sender"]),
                            Sender = GetSender(Convert.ToInt32(reader["Sender"])),
                            Content = reader["Content"].ToString(),
                            Important = Convert.ToBoolean(reader["Important"]),
                            Tags = GetTags(Convert.ToInt32(reader["ID"]))
                        });
                    }
                }
            }
            return messages;
        }

        //Получение писем определенного отправителя
        public IEnumerable<Message> GetMessageList(int sender_id)
        {
            List<Message> messages = new List<Message>();
            using (SqlConnection connection = GetSqlConnection())
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT * FROM Messages WHERE Sender = {sender_id}", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        messages.Add(new Message
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Title = reader["Title"].ToString(),
                            DateTime = Convert.ToDateTime(reader["Datetime"]),
                            Recipient = reader["Recipient"].ToString(),
                            SenderID = Convert.ToInt32(reader["Sender"]),
                            Sender = GetSender(Convert.ToInt32(reader["Sender"])),
                            Content = reader["Content"].ToString(),
                            Important = Convert.ToBoolean(reader["Important"]),
                            Tags = GetTags(Convert.ToInt32(reader["ID"]))
                        });
                    }
                }
            }
            return messages;
        }

        //Получение данных определенного письма
        public Message GetMessage(int message_id)
        {
            Message message = new Message();
            using (SqlConnection connection = GetSqlConnection())
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT * FROM Messages WHERE ID = {message_id}", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        message.ID = Convert.ToInt32(reader["ID"]);
                        message.Title = reader["Title"].ToString();
                        message.DateTime = Convert.ToDateTime(reader["Datetime"]);
                        message.Recipient = reader["Recipient"].ToString();
                        message.SenderID = Convert.ToInt32(reader["Sender"]);
                        message.Sender = GetSender(Convert.ToInt32(reader["Sender"]));
                        message.Content = reader["Content"].ToString();
                        message.Important = Convert.ToBoolean(reader["Important"]);
                        message.Tags = GetTags(message_id);
                    }
                }
            }
            return message;
        }

        //Получение данных определенного отправителя
        public Sender GetSender(int sender_id)
        {
            Sender sender = new Sender();
            using (SqlConnection connection = GetSqlConnection())
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT * FROM Senders WHERE ID = {sender_id}", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        sender.ID = reader.GetInt32(0);
                        sender.Firstname = reader.GetString(1);
                        sender.Lastname = reader.GetString(2);
                        sender.Address = reader.GetString(3);
                    }
                }
            }
            return sender;
        }

        //Получение всех отправителей
        public IEnumerable<Sender> GetSenderList()
        {
            List<Sender> senders = new List<Sender>();
            using (SqlConnection connection = GetSqlConnection())
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT * FROM Senders", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        senders.Add(new Sender
                        {
                            ID = reader.GetInt32(0),
                            Firstname = reader.GetString(1),
                            Lastname = reader.GetString(2),
                            Address = reader.GetString(3)
                        });
                        
                    }
                }
            }
            return senders;
        }

        //Получение всех тэгов привязанных к письму
        public IEnumerable<Tag> GetTags(int message_id)
        {
            List<Tag> tags = new List<Tag>();
            using (SqlConnection connection = GetSqlConnection())
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT TagsInMessage.TagID, TagsInMessage.MessageID, Tags.Tag FROM Tags INNER JOIN TagsInMessage ON Tags.ID = TagsInMessage.TagID WHERE TagsInMessage.MessageID = {message_id}", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tags.Add(new Tag
                        {
                            ID = reader.GetInt32(0),
                            MessageID = reader.GetInt32(1),
                            TagName = reader.GetString(2)
                        });

                    }
                }
            }
            return tags;
        }

        //Получение данных о тэге по имени для проверки наличия тэга в БД. Не до конца реализовано
        public Tag GetTagByName(string tag_name)
        {
            Tag tag = new Tag();
            using (SqlConnection connection = GetSqlConnection())
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT * FROM Tags WHERE Tags.Tag = '{tag_name}'", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tag.ID = reader.GetInt32(0);
                        tag.TagName = reader.GetString(1);
                    }
                }
            }
            return tag;
        }
        //Изменение письма 
        public void EditMessage(Message message)
        {
            using (SqlConnection connection = GetSqlConnection())
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"UPDATE Messages SET Title = '{message.Title}', Datetime = SYSDATETIME(), Content = '{message.Content}', Important = {Convert.ToInt32(message.Important)} WHERE ID = {message.ID}", connection);
                command.ExecuteNonQuery();
            }
        }

        //Поиск по дате регистрации за период
        public string GetMessageListByDatetime(DateTime FromDateTime, DateTime ToDateTime)
        {
            return $"SELECT * FROM Messages WHERE Messages.Datetime >= '{FromDateTime}' AND Messages.Datetime <= '{ToDateTime}'";
        }

        public string GetMessageListByRecipient(string recipient_address)
        {
            return $"SELECT * FROM Messages WHERE Messages.Recipient = '{recipient_address}'";
        }

        public string GetMessageListBySender(Sender sender)
        {
            return $"SELECT * FROM Messages INNER JOIN Senders ON Messages.Sender = Senders.ID WHERE Senders.Address = '{sender.Address}'";
        }

        public string GetMessageListByTag(string tag_name)
        {
            return $"SELECT * FROM Messages INNER JOIN TagsInMessage ON Messages.ID = TagsInMessage.MessageID INNER JOIN Tags ON Tags.ID = TagsInMessage.TagID WHERE Tags.Tag = '{tag_name}'";
        }

        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));
        }

    }
}
