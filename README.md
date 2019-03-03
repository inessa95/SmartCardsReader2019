# SmartCardsReader2019
smart Cards project 
Université de Nice-Sophia Antipolis
March 3, 2019 
BOUARROUJ Ines
GONZAGA DOS SANTOS Michel

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
NDEF Card Android Emulator
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

# Prerequisites: 

- Android Studio + SDK 19 and 28

# Development

- The first Step was to creat the anroid project
	- language: Java
	- Min API: 19(Adroid 4.4 - KitKat)

- The imports used in this code are: 
	android.nfc.NdefMessage;
 	android.nfc.NdefRecord;
	android.nfc.cardemulation.HostApduService;
 	android.os.Bundle;

Commands based on Tag NFC Forum Type 4:

- SELECT_APP (byte array) = /*CLA*/ (byte)0x00, /*INS*/ (byte)0xa4, /*P1*/ (byte)0x04, /*P2*/ (byte)0x00,
/*Lc*/ (byte)0x07, /*Data*/ (byte)0xd2, (byte)0x76, (byte)0x00, (byte)0x00, (byte)0x85, (byte)0x01, (byte)0x01,
/*Le*/ (byte)0x00

- SELECT_CC_FILE = /*CLA*/ (byte)0x00, /*INS*/ (byte)0xa4, /*P1*/ (byte)0x00, /*P2*/ (byte)0x0c, /*Lc*/(byte)0x02,
/*Data*/ (byte)0xe1, (byte)0x03

- SELECT_NDEF_FILE = /*CLA*/ (byte)0x00, /*INS*/ (byte)0xa4, /*P1*/ (byte)0x00, /*P2*/ (byte)0x0c, /*Lc*/(byte)0x02,
/*File ID*/ (byte)0x81, (byte)0x01,


- SW responses: 

	- Succes response:
            - (byte)0x90, (byte)0x00
        
	- Failure:

        // Unkown CLA 
            (byte)0x6e, (byte)0x00

        // Unkown INS 
            (byte)0x6d, (byte)0x00

        // Unkown Lc 
            (byte)0x67, (byte)0x00

        // Unkown Le 
            (byte)0x6c, (byte)0x00,

        // Unkown AID/LID
            (byte)0x6a, (byte)0x82,
    
        // No compilable state 
            (byte)0x69, (byte)0x86,

        // Unkown P1/P2 (SELECT)
            (byte)0x6a, (byte)0x86,

        // Unkown P1/P2 (READ/UPDATE BINARY)
            (byte)0x6b, (byte)0x00,

        // Unkown Offset/Lc 
            (byte)0x6a, (byte)0x87,
 


- The order of the commands is presented bellow:
	- SELECT APPLICATION
	- SELECT CC FILE
        - READ CC FILE
        - SELECT NDEF FILE
        - READ NDFE FILE
        - UPDATE NDFE FILE

 For each of the previous stages we checked the possible errors, returning the most 
 appropriated SW respose, to do so we used an conditional structure composed by if, 
 else if and else commands, the conditons were evaluated based on caparision with 
commandApdu.
