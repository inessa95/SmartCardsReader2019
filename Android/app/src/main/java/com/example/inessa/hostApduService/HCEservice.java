package com.example.inessa.hostApduService;

import android.annotation.TargetApi;
import android.nfc.cardemulation.HostApduService;
import android.os.Build;
import android.os.Bundle;
import android.support.annotation.RequiresApi;


import java.util.Arrays;


@TargetApi(Build.VERSION_CODES.KITKAT)
@RequiresApi(api = Build.VERSION_CODES.KITKAT)

public class HCEservice extends HostApduService {
//command for select application
    private final static byte[] SELECT_APP = new byte[] {(byte)0x00, (byte)0xa4, (byte)0x04, (byte)0x00,
            (byte)0x07, (byte)0xd2, (byte)0x76, (byte)0x00, (byte)0x00, (byte)0x85, (byte)0x01, (byte)0x01,
            (byte)0x00,
    };
    //data field of select application
    private final static byte[] data= new byte[] { (byte) 0xd2 , (byte)0x76, (byte)0x00, (byte)0x00, (byte)0x85, (byte)0x01, (byte)0x01  };


    //data field of select cc file

//command for select cc file
    private final static byte[] SELECT_CC_FILE = new byte[] {(byte)0x00, (byte)0xa4, (byte)0x00, (byte)0x0c,
            (byte)0x02, (byte)0xe1, (byte)0x03,
    };
//command for selecting ndef file
    private final static byte[] SELECT_NDEF_FILE = new byte[] {(byte)0x00, (byte)0xa4, (byte)0x00, (byte)0x0c,
            (byte)0x02, (byte)0xe1, (byte)0x04,
    };

//managing errors

//CLAerrorCode
    private final static byte[] ClaError = new byte[] {
            (byte)0x6e, (byte)0x00,
    };
 //INSerrorCode
    private final static byte[] INSERROR = new byte[] {
            (byte)0x6d, (byte)0x00,
    };
    //lc error code
    private final static byte[] lcError = new byte[] {
            (byte)0x67, (byte)0x00,
    };
    //le error code
    private final static byte[] leError = new byte[] {
            (byte)0x6c, (byte)0x00,
    };
//aid error code
    private final static byte[] AIDerror = new byte[] {
            (byte)0x6A, (byte)0x82,
    };
    //p1 p2 error code when trying to select a file
    private final static byte[] p1p2errorSelect = new byte[] {
            (byte)0x69, (byte)0x86,
    };
    //p1 p2 error code when trying to read or update the file
    private final static byte[] p1p2errorReadUPDATE = new byte[] {
            (byte)0x6A, (byte)0x86,
    };
    // offset / lc error code
    private final static byte[] LCERROR = new byte[] {
            (byte)0x6A, (byte)0x87,
    };
// offset / lc error code
private final static byte[] LEERROR = new byte[] {
        (byte)0x6C, (byte)0x00,
};

//return 9000  success-code
    private final static byte[] succes = new byte[] {
            (byte)0x90, (byte)0x00,
    };
//retuning LID FAILURE CODE
    private final static byte[] failure = new byte[] {
            (byte)0x6a, (byte)0x82,
    };

//cc file
    private final static byte[] CC_FILE = new byte[] {
            0x00, 0x0f, // CCLEN
            0x20, // Mapping Version
            0x00, 0x3b, // Maximum R APDU data size
            0x00, 0x34, // Maximum C APDU data size
            0x04, 0x06,
            (byte)0xe1, 0x04, // NDEF File Identifier
           0x08, 0x00, 0x00, 0x00
    };

//ndef file
    private final static byte[] NDEFFILE = new byte[] {
            (byte) 0x91, 0x01, // CCLEN
            (byte) 0x0a, // Mapping Version
            0x55, 0x01, // Maximum R APDU data size
            0x61, 0x70, // Maximum C APDU data size
            0x70, 0x6c, 0x65, 0x2e, 0x63, 0x6f, 0x6d, 0x11, 0x01, 0x14, 0x54, 0x02, 0x66, 0x72,0x4c, 0x61 , 0x20, 0x62, 0x65, 0x6c, 0x6c, 0x65, 0x20, 0x68, 0x69, 0x73, 0x74, 0x6f, 0x69, 0x72, 0x65, 0x51, 0x00, 0x08, 0x50,
            0x4f, 0x59, 0x54, 0x45, 0x43, 0x48


    };






//to select files in proper order
    private boolean mAppSelected;


    private boolean mCcSelected;


    private boolean mNdefSelected;

    @Override
    public void onCreate() {
        super.onCreate();


        mAppSelected = false;
        mCcSelected = false;
        mNdefSelected = false;



    }


    @Override
    public byte[] processCommandApdu(byte[] commandApdu, Bundle extras) {

//returning cla eroor
        if((commandApdu[0]!=(byte)0x00 ))
        {
            return ClaError;
        }
//returning insert error
        if( (commandApdu[1]!=(byte)0xa4  ) && (commandApdu[1]!=(byte)0xb0 ) && (commandApdu[1]!= (byte)0xd6 ) )
        {
            return INSERROR;
        }
//returning p1 p2 error when trying to select
        if ( (commandApdu[2] != (byte) 0x04 && commandApdu[3] != 0x00 && commandApdu[2]!=(byte) 0x00 && commandApdu[3]!=0x0c && mNdefSelected)  || (commandApdu[2] != (byte) 0x04 && commandApdu[3] != 0x00 && commandApdu[2]!=(byte) 0x00 && commandApdu[3]!=0x0c && mCcSelected) ) {
            return p1p2errorSelect;
        }

//returning p1 p2 error when when trying to read data -out of range
//returning le error
        if( (byte)0x01 > commandApdu[6] && commandApdu[6] > (byte)0xff  )
        {
            return leError;
        }
        //returning error code when lc in incorrect
    if(commandApdu[4] != (byte)0x07 && commandApdu[4] != (byte)0x02  &&  commandApdu[4] != (byte)0x00    )
    {
        return  lcError;
    }


        //returning
//checking if selecting happened according to the proper order

        if (Arrays.equals(SELECT_APP, commandApdu)) {

            mAppSelected = true;
            mCcSelected = false;
            mNdefSelected = false;
            return succes;
        }

        else if (mAppSelected && Arrays.equals(SELECT_CC_FILE, commandApdu)) {

            mCcSelected = true;
            mNdefSelected = false;
            return succes;
        } else if (mAppSelected && Arrays.equals(SELECT_NDEF_FILE, commandApdu))
        {

            mCcSelected = false;
            mNdefSelected = true;
            return succes;
        }

        else if (commandApdu[0] == (byte)0x00 && commandApdu[1] == (byte)0xb0) {



            int offset = (0x00ff & commandApdu[2]) * 256 + (0x00ff & commandApdu[3]);
            int le = 0x00ff & commandApdu[4];


            byte[] responseApdu = new byte[le + succes.length];
//copying the ccfile on the card
            if (mCcSelected && offset == 0 && le == CC_FILE.length) {

                System.arraycopy(CC_FILE, offset, responseApdu, 0, le);
                System.arraycopy(succes, 0, responseApdu, le, succes.length);

                return responseApdu;
            }
            //copying the ndef file to the card
            else if (mNdefSelected) {

                    System.arraycopy(NDEFFILE, offset, responseApdu, 0, le);
                    System.arraycopy(succes, 0, responseApdu, le, succes.length);

                    return responseApdu;

            }


        }


        return failure;
    }


    @Override
    public void onDeactivated(int reason) {

        mAppSelected = false;
        mCcSelected = false;
        mNdefSelected = false;
    }
}