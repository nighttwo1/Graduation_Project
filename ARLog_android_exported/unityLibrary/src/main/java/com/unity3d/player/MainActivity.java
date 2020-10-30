package com.unity3d.player;

import android.app.Activity;
import android.content.Intent;
import android.database.Cursor;
import android.net.Uri;
import android.os.Bundle;
import android.provider.MediaStore;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import androidx.annotation.Nullable;

import com.unity3d.player.vuforiawebservicesAPI.PostNewTarget;

import org.json.JSONException;

import java.io.File;
import java.io.IOException;
import java.net.URISyntaxException;

public class MainActivity extends Activity {

    private final int GET_GALLERY_IMAGE = 200;

    public static String selectedimagePath;

    public static String selectedimageName;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        EditText e = findViewById(R.id.et);
        findViewById(R.id.button).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if(e.getText().toString()!=null){
                    selectedimageName = e.getText().toString();
                    Intent intent = new Intent(Intent.ACTION_PICK);
                    intent.setType("image/*");
                    startActivityForResult(intent, GET_GALLERY_IMAGE);
                }else{
                    Toast.makeText(getApplicationContext(), "업로드할 이미지 이름을 설정하세요.", Toast.LENGTH_SHORT).show();
                }
            }
        });
        findViewById(R.id.button2).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(getApplicationContext(), UnityPlayerActivity.class);
                startActivity(intent);
            }
        });
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, @Nullable Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if(requestCode == GET_GALLERY_IMAGE && resultCode == RESULT_OK && data!=null && data.getData()!=null){
            Uri selectedImageUri = data.getData();
            selectedimagePath = getRealPathFromURI(selectedImageUri);
            Log.d("Selected Image Path", selectedimagePath);


            try {
                PostNewTarget.main();
            } catch (URISyntaxException e) {
                e.printStackTrace();
            } catch (IOException e) {
                e.printStackTrace();
            } catch (JSONException e) {
                e.printStackTrace();
            }


        }else{
            Log.d("Getting image", "Error");
        }
    }

    public String getRealPathFromURI(Uri contentUri) {

        String[] proj = { MediaStore.Images.Media.DATA };

        Cursor cursor = getApplicationContext().getContentResolver().query(contentUri, proj, null, null, null);
        cursor.moveToNext();
        String path = cursor.getString(cursor.getColumnIndex(MediaStore.MediaColumns.DATA));
        Uri uri = Uri.fromFile(new File(path));

        Log.d("TAG", "getRealPathFromURI(), path : " + uri.toString());

        cursor.close();
        return path;
    }

}
