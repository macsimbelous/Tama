using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace Rena
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                MessageBox.Show("Не заданы параметры!");
                return;
            }
            if (args.Length == 1)
            {
                md5_enc(args[0]);
                return;
            }
            if (args.Length > 1)
            {
                if (args[1].CompareTo("--rename") == 0)
                {
                    md5_enc_rename(args[0]);
                }
                return;
            }
        }
        static void md5_enc(string file)
        {
            string hash_str = null;
            string new_file = null;
            MD5 hash_enc = MD5.Create();
            if (IsImageFile(file))
            {
                FileStream fsData = new FileStream(file, FileMode.Open, FileAccess.Read);
                byte[] hash = hash_enc.ComputeHash(fsData);
                fsData.Close();
                string t = file.Substring(0, file.LastIndexOf('\\') + 1);
                hash_str = BitConverter.ToString(hash).Replace("-", string.Empty);
                new_file = t + hash_str.ToLower() + file.Substring(file.LastIndexOf('.'));
                if (MessageBox.Show("Хэш файла: " + hash_str + "\nКопировать в буфер?", "Хэш подсчитан", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Clipboard.SetText(hash_str);
                }
            }
            else
            {
                MessageBox.Show("Этот фаил не является изображением!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        static void md5_enc_rename(string file)
        {
            if (MessageBox.Show("Переименовать фаил?", "Запрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string hash_str = null;
                string new_file = null;
                MD5 hash_enc = MD5.Create();
                if (IsImageFile(file))
                {
                    FileStream fsData = new FileStream(file, FileMode.Open, FileAccess.Read);
                    byte[] hash = hash_enc.ComputeHash(fsData);
                    fsData.Close();
                    string t = file.Substring(0, file.LastIndexOf('\\') + 1);
                    hash_str = BitConverter.ToString(hash).Replace("-", string.Empty);
                    new_file = t + hash_str.ToLower() + file.Substring(file.LastIndexOf('.'));
                    try
                    {
                        System.IO.File.Move(file, new_file.ToLower());
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("Фаил с токим именем уже существует!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Этот фаил не является изображением!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        static bool IsImageFile(string s)
        {
            int t = s.LastIndexOf('.');
            if (t >= 0)
            {
                string ext = s.Substring(t).ToLower();
                switch (ext)
                {
                    case ".jpg":
                        return true;
                    //break;
                    case ".jpeg":
                        return true;
                    //break;
                    case ".png":
                        return true;
                    //break;
                    case ".bmp":
                        return true;
                    //break;
                    case ".gif":
                        return true;
                    //break;
                    case ".tif":
                        return true;
                    //break;
                    case ".tiff":
                        return true;
                    //break;

                }
            }
            return false;
        }
    }
}