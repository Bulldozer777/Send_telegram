using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TeleSharp.TL;
using TeleSharp.TL.Messages;
using TLSharp.Core;
using TLSharp.Core.Utils;

namespace Send_telegram
{
    public partial class Form1 : Form
    {
        private readonly static int apiId = 8164488;
        private readonly static string apiHash = "ee92079c288690b01e5c22d018789808";
        static string phoneNumber = "";
        TelegramClient client = new TelegramClient(apiId, apiHash);
        int ID = 0;
        public Form1()
        {
            InitializeComponent();
            AutoCompleteStringCollection source_1 = new AutoCompleteStringCollection()
        {
            "+79605162371"
        };
            textBox2.AutoCompleteCustomSource = source_1;
            textBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection source_2 = new AutoCompleteStringCollection()
            {
                "Данила",
            };
            textBox3.AutoCompleteCustomSource = source_2;
            textBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox3.AutoCompleteSource = AutoCompleteSource.CustomSource;
            Button[] Mass_Button = new Button[61] { button1, button2, button3, button4, button5,button6, button7, button8, button9, button10, button11, button12,
            button13, button14, button15, button16, button17, button18, button19,button20, button21,button22,
            button23, button25, button27, button28, button29, button30, button31, button32,
            button34, button36, button38, button40, button41, button42, button43, button44,
            button45, button47, button49, button51, button53, button54, button55, button56,
            button57, button58, button60, button62, button64, button66, button67, button68,
             button69, button70, button71, button73, button75, button77, button79};

            foreach (Button button in Mass_Button)
            {
                CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                         // отменяет отслеживание ошибок,
                                                         // но дает передать компоненты формы в другой поток 
                button.Click += async (s, e) =>
                {
                    for (int i = 0; i < Mass_Button.Length; i++)
                    {
                        if (button == Mass_Button[i])
                        {
                            await Task.Run(() => Message(button));
                        }
                    }
                };
            }
        }
        async public void Message(Button button)
        {

            try
            {
                await client.ConnectAsync();
                await client.SendMessageAsync(new TLInputPeerUser() { UserId = ID }, button.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }

        }
        async private void button13_Click(object sender, EventArgs e)
        {
            await Task.Run(() => AuthenticationAsync());
        }
        async public void AuthenticationAsync()
        {
            phoneNumber = textBox2.Text;
            await client.ConnectAsync();
            var hash = await client.SendCodeRequestAsync(phoneNumber);
            var code = textBox1.Text;
            var user = await client.MakeAuthAsync(phoneNumber, hash, code);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            SendAsync();
        }
        async public void SendAsync()
        {
            if (textBox3.Text != "")
            {
                await client.ConnectAsync();
                var result = await client.GetContactsAsync();
                var user = result.Users
                    .Where(x => x.GetType() == typeof(TLUser))
                    .Cast<TLUser>()
                    .FirstOrDefault(x => x.FirstName == textBox3.Text);
                ID = user.Id;
            }
            else
                MessageBox.Show($"Поле для ввода имени получателя сообщения - пустое");
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    SendAsync();
        //}
    }
}
