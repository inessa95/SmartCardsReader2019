using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CSharpNETTestASKCSCDLL
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}
		string HexStringToString(string hexString)
		{
			if (hexString == null || (hexString.Length & 1) == 1)
			{
				throw new ArgumentException();
			}
			var sb = new StringBuilder();
			for (var i = 0; i < hexString.Length; i += 2)
			{
				var hexChar = hexString.Substring(i, 2);
				sb.Append((char)Convert.ToByte(hexChar, 16));
			}
			return sb.ToString();
		}

		public static byte[] StringToByteArray(String hex)
		{
			int NumberChars = hex.Length / 2;
			byte[] bytes = new byte[NumberChars];
			using (var sr = new System.IO.StringReader(hex))
			{
				for (int i = 0; i < NumberChars; i++)
					bytes[i] =
					  Convert.ToByte(new string(new char[2] { (char)sr.Read(), (char)sr.Read() }), 16);
			}
			return bytes;
		}

		private void Button1_Click(object sender, EventArgs e)
		{
			AskReaderLib.CSC.sCARD_SearchExtTag SearchExtender;
			int Status; int Status1; int Status2, Status3, Status4;
			byte[] ATR;
			ATR = new byte[200];
			int lgATR;
			lgATR = 200;
			int Com = 0;
			int SearchMask;

			txtCom.Text = "";
			txtCard.Text = "";

			try
			{
				AskReaderLib.CSC.SearchCSC();
				// user can also use line below to speed up coupler connection
				//AskReaderLib.CSC.Open ("COM2");

				// Define type of card to be detected: number of occurence for each loop
				SearchExtender.CONT = 0;
				SearchExtender.ISOB = 2;
				SearchExtender.ISOA = 2;
				SearchExtender.TICK = 1;
				SearchExtender.INNO = 2;
				SearchExtender.MIFARE = 0;
				SearchExtender.MV4k = 0;
				SearchExtender.MV5k = 0;
				SearchExtender.MONO = 0;

				// Define type of card to be detected
				Status = AskReaderLib.CSC.CSC_EHP_PARAMS_EXT(1, 1, 0, 0, 0, 0, 0, 0, null, 0, 0);
				Status1 = AskReaderLib.CSC.CSC_EHP_PARAMS_EXT(1, 1, 0, 0, 0, 0, 0, 0, null, 0, 0);
				Status2 = AskReaderLib.CSC.CSC_EHP_PARAMS_EXT(1, 1, 0, 0, 0, 0, 0, 0, null, 0, 0);
				Status4 = AskReaderLib.CSC.CSC_EHP_PARAMS_EXT(1, 1, 0, 0, 0, 0, 0, 0, null, 0, 0);
				SearchMask = AskReaderLib.CSC.SEARCH_MASK_ISOB | AskReaderLib.CSC.SEARCH_MASK_ISOA;
				Status = AskReaderLib.CSC.SearchCardExt(ref SearchExtender, SearchMask, 1, 20, ref Com, ref lgATR, ATR);
				Status1 = AskReaderLib.CSC.SearchCardExt(ref SearchExtender, SearchMask, 1, 20, ref Com, ref lgATR, ATR);
				Status2 = AskReaderLib.CSC.SearchCardExt(ref SearchExtender, SearchMask, 1, 20, ref Com, ref lgATR, ATR);

				Status4 = AskReaderLib.CSC.SearchCardExt(ref SearchExtender, SearchMask, 1, 20, ref Com, ref lgATR, ATR);
				if (Status4 != AskReaderLib.CSC.RCSC_Ok)
					txtCom.Text = "Error :" + Status4.ToString("X");
				else
					txtCom.Text = Com.ToString("X");

				Status3 = AskReaderLib.CSC.SearchCardExt(ref SearchExtender, SearchMask, 1, 20, ref Com, ref lgATR, ATR);
				if (Status3 != AskReaderLib.CSC.RCSC_Ok)
					txtCom.Text = "Error :" + Status3.ToString("X");
				else
					txtCom.Text = Com.ToString("X");


				if (Status != AskReaderLib.CSC.RCSC_Ok)
					txtCom.Text = "Error :" + Status.ToString("X");
				else
					txtCom.Text = Com.ToString("X");

				if (Status1 != AskReaderLib.CSC.RCSC_Ok)
					txtCom.Text = "Error :" + Status.ToString("X");
				else
					txtCom.Text = Com.ToString("X");

				if (Status2 != AskReaderLib.CSC.RCSC_Ok)
					txtCom.Text = "Error :" + Status.ToString("X");
				else
					txtCom.Text = Com.ToString("X");

				if (Com == 2)
				{
					txtCard.Text = "ISO14443A-4 no Calypso";
		//commmand to select application 
					Byte[] ByBuFIn = new Byte[] { 0x00/*CLA*/, 0xA4/*IWS*/, 0x04/*P1*/, 0x00/*P2*/, 0x07/*lc*/, 0xD2, 0x76, 0x00, 0x00, 0x85, 0x01, 0x01/*data*/, 0x00/*le*/ };
					//command to select cc file 
					Byte[] ByBuFIn2 = new Byte[] { 0x00/*CLA*/, 0xA4/*IWS*/, 0x00/*P1*/, 0x0C/*P2*/, 0x02/*lc*/, 0xE1, 0x03 };
					//command to read cc file 
					Byte[] ByBuFIn3 = new Byte[] { 0x00/*CLA*/, 0xB0/*IWS*/, 0x00/*offset*/, 0x00/*P2*/, 0x0F };//commande read cc file
					//command to select ndef 																						
					Byte[] ByBuFIn4 = new Byte[] { 0x00/*CLA*/, 0xA4/*IWS*/, 0x00/*offset*/, 0x0C/*P2*/, 0x02, 0XE1, 0x04 };//diapo 89 en bas
								//command to read ndef																							//commande pour la lecture du fichier ndef
					Byte[] ByBuFIn5 = new Byte[] { 0x00/*CLA*/, 0xB0/*IWS*/, 0x00/*offset*/, 0x00/*P2*/, 0x24 };
				//command to update ndef
					Byte[] ByBuFIn6 = new Byte[] { 0x00/*CLA*/, 0xD6/*InS*/, 0x00/*offset*/, 0x00/*P2*/, 0x24, 0x91, 0x01, 0x0A, 0x55, 0x01, 0x61, 0x70, 0x70, 0x6C, 0x65, 0x2E, 0x63, 0x6F, 0x6D, 0x11, 0x01, 0x14, 0x54, 0x02, 0x66, 0x72, 0x68, 0x74, 0x74,  0x70,  0x73 , 0x3a, 0x2f , 0x2f, 0x77 , 0x77, 0x77 , 0x2e,  0x67, 0x6f , 0x6f, 0x67, 0x6c, 0x65, 0x2e, 0x63, 0x6f , 0x6d, 0x51, 0x00, 0x08, 0x50, 0x4F, 0x4C, 0x59, 0x54, 0x45, 0x43, 0x48, 0x4e, 0x69, 0x63, 0x65 };


					int iLenOUT = 300;
					Byte[] byBufOut = new Byte[iLenOUT];
					Byte[] byBufOut2 = new Byte[iLenOUT];
					Byte[] byBufOut3 = new Byte[iLenOUT];
					Byte[] byBufOut4 = new Byte[iLenOUT];
					Byte[] byBufOut5 = new Byte[iLenOUT];
					Byte[] byBufOut6 = new Byte[iLenOUT];
					if ((Status == AskReaderLib.CSC.CSC_ISOCommand(ByBuFIn, ByBuFIn.Length, byBufOut, ref iLenOUT)))
					{


						Console.WriteLine("\n**** select Application \n");

						Console.WriteLine(BitConverter.ToString(byBufOut).Replace("-", string.Empty));

					}

					if ((Status1 == AskReaderLib.CSC.CSC_ISOCommand(ByBuFIn2, ByBuFIn2.Length, byBufOut2, ref iLenOUT)))
					{



						Console.WriteLine("\n**** select cc file \n");

						Console.WriteLine(BitConverter.ToString(byBufOut2).Replace("-", string.Empty));

					}
					if ((Status2 == AskReaderLib.CSC.CSC_ISOCommand(ByBuFIn3, ByBuFIn3.Length, byBufOut3, ref iLenOUT)))
					{



						Console.WriteLine("\n**** Read All binary cc file   \n");

						Console.WriteLine(BitConverter.ToString(byBufOut3).Replace("-", string.Empty));
						String a = (BitConverter.ToString(byBufOut3).Replace("-", string.Empty)).Substring(8, 4);
						String b = (BitConverter.ToString(byBufOut3).Replace("-", string.Empty)).Substring(12, 4);
						String c = (BitConverter.ToString(byBufOut3).Replace("-", string.Empty)).Substring(16, 16);
						String d = c.Substring(4, 4);

						String f = c.Substring(8, 4);



						Console.WriteLine("read max le(max data size can be read from  type 4) " + a);
						Console.WriteLine("read max lc( max data can be sent to type 4" + b);
						Console.WriteLine("read ndef file Control" + c);
						Console.WriteLine("FILE IDENTIFIER (lid) " + d);
						Console.WriteLine(" max ndef file size " + f);
					}

					if ((Status3 == AskReaderLib.CSC.CSC_ISOCommand(ByBuFIn4, ByBuFIn4.Length, byBufOut4, ref iLenOUT)))
					{



						Console.WriteLine("\n**** select file ndef  \n");

						Console.WriteLine(BitConverter.ToString(byBufOut4).Replace("-", string.Empty));

					}

				

					/*-----------------------writing tag--------------------------------*/

					if ((Status4 == AskReaderLib.CSC.CSC_ISOCommand(ByBuFIn6, ByBuFIn6.Length, byBufOut6, ref iLenOUT)))
					{
					
					Console.WriteLine("\n**** Write message in tag ndef \n");


					}}
				else if (Com == 3)
					txtCard.Text = "INNOVATRON";
				else if (Com == 4)
					txtCard.Text = "ISOB14443B-4 Calypso";
				else if (Com == 5)
					txtCard.Text = "Mifare";
				else if (Com == 6)
					txtCard.Text = "CTS or CTM";
				else if (Com == 8)
					txtCard.Text = "ISO14443A-3 ";
				else if (Com == 9)
					txtCard.Text = "ISOB14443B-4 Calypso";
				else if (Com == 12)
					txtCard.Text = "ISO14443A-4 Calypso";
				else if (Com == 0x6F)
					txtCard.Text = "Card not found";
				else
					txtCard.Text = "";
			}
			catch
			{
				MessageBox.Show("Error on trying do deal with reader");
			}
			AskReaderLib.CSC.Close();
		}
	}
}