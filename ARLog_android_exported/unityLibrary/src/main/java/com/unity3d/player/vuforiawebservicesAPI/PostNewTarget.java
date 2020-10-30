package com.unity3d.player.vuforiawebservicesAPI;

import android.util.Log;
import android.widget.Toast;

import org.apache.commons.codec.binary.Base64;
import org.apache.commons.io.FileUtils;
import org.apache.http.HttpResponse;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.client.methods.HttpUriRequest;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.impl.cookie.DateUtils;
import org.apache.http.message.BasicHeader;
import org.apache.http.util.EntityUtils;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.File;
import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.net.URI;
import java.net.URISyntaxException;
import java.util.Date;

import static com.unity3d.player.MainActivity.selectedimageName;
import static com.unity3d.player.MainActivity.selectedimagePath;


//import com.ptc.vuforia.CloudRecognition.utils.SignatureBuilder;


// See the Vuforia Web Services Developer API Specification - https://library.vuforia.com/articles/Solution/How-To-Use-the-Vuforia-Web-Services-API#How-To-Add-a-Target

public class PostNewTarget implements TargetStatusListener {

	//Server Keys
	private String accessKey = "48d76c1070b9591e26fa4ab86009a8012381ec53";
	private String secretKey = "c31e5bb93f19ca8dcd1bb823a1046ecccd40fd6e";
	
	private String url = "https://vws.vuforia.com";
	private String targetName = selectedimageName;
	//private String imageLocation = "storage/emulated/0/Pictures/Everytime/everytime-1600234207064.jpg";

	private TargetStatusPoller targetStatusPoller;
	
	private final float pollingIntervalMinutes = 60;//poll at 1-hour interval
	
	private String postTarget() throws URISyntaxException, ClientProtocolException, IOException, JSONException {
		final HttpPost postRequest = new HttpPost();
		final HttpClient client = new DefaultHttpClient();
		postRequest.setURI(new URI(url + "/targets"));
		final String[] uniqueTargetId = {null};

		new Thread(){
			@Override
			public void run() {
				JSONObject requestBody = new JSONObject();

				try {
					setRequestBody(requestBody);
				} catch (IOException e) {
					e.printStackTrace();
				} catch (JSONException e) {
					e.printStackTrace();
				}
				try {
					postRequest.setEntity(new StringEntity(requestBody.toString()));
				} catch (UnsupportedEncodingException e) {
					e.printStackTrace();
				}
				setHeaders(postRequest); // Must be done after setting the body

				HttpResponse response = null;
				try {
					response = client.execute(postRequest);
					System.out.println("response: "+response.getEntity());
				} catch (IOException e) {
					e.printStackTrace();
				}
				Log.d("T??", response.getStatusLine().getStatusCode()+"");
				String responseBody = null;
				try {
					responseBody = EntityUtils.toString(response.getEntity());
				} catch (IOException e) {
					e.printStackTrace();
				}
				System.out.println("responseBody: "+responseBody);

				JSONObject jobj = null;
				try {
					jobj = new JSONObject(responseBody);
					System.out.println("jobj: "+ jobj.toString());
				} catch (JSONException e) {
					e.printStackTrace();
				}

				try {
					uniqueTargetId[0] = jobj.has("target_id") ? jobj.getString("target_id") : "";
					System.out.println("\nCreated target with id2: " + uniqueTargetId[0]);
				} catch (JSONException e) {
					e.printStackTrace();
				}
				System.out.println("\nCreated target with id: " + uniqueTargetId[0]);
			}
		}.start();


		return uniqueTargetId[0];
	}
	
	private void setRequestBody(JSONObject requestBody) throws IOException, JSONException {
		File imageFile = new File(selectedimagePath);
		if(!imageFile.exists()) {
			System.out.println("File location does not exist!");
			System.exit(1);
		}
		/*
		int size = (int) imageFile.length();
		Log.d("size", size+"");
		byte[] bytes = new byte[size];
		try {
			BufferedInputStream buf = new BufferedInputStream(new FileInputStream(imageFile));
			buf.read(bytes, 0, bytes.length);
			buf.close();
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		 */

		byte[] image = FileUtils.readFileToByteArray(imageFile);
		requestBody.put("name", targetName); // Mandatory
		requestBody.put("width", 320.0); // Mandatory
		requestBody.put("image", Base64.encodeBase64(image)); // Mandatory

		//requestBody의 image부분 수정 필요!!!
		//requestBody.put("image", new String(Base64.encodeBase64(image), "UTF-8"));

		Log.d("image", String.valueOf(Base64.encodeBase64(image)));
		//requestBody.put("active_flag", 1); // Optional
		//requestBody.put("application_metadata", Base64.encodeBase64("Vuforia test metadata".getBytes())); // Optional
	}
	
	private void setHeaders(HttpUriRequest request) {
		SignatureBuilder sb = new SignatureBuilder();
		request.setHeader(new BasicHeader("Host", url));
		request.setHeader(new BasicHeader("Date", DateUtils.formatDate(new Date()).replaceFirst("[+]00:00$", "")));
		request.setHeader("Authorization", "VWS " + accessKey + ":" + sb.tmsSignature(request, secretKey));
		request.setHeader(new BasicHeader("Content-Type", "application/json"));
	}
	
	/**
	 * Posts a new target to the Cloud database; 
	 * then starts a periodic polling until 'status' of created target is reported as 'success'.
	 */
	public void postTargetThenPollStatus() {
		String createdTargetId = "";
		try {
			createdTargetId = postTarget();
			//Log.d("createdTargetId", createdTargetId);
		} catch (URISyntaxException | IOException | JSONException | NullPointerException e) {
			e.printStackTrace();
			return;
		}
		// Poll the target status until the 'status' is 'success'
		// The TargetState will be passed to the OnTargetStatusUpdate callback 
		if (createdTargetId != null && !createdTargetId.isEmpty()) {
			targetStatusPoller = new TargetStatusPoller(pollingIntervalMinutes, createdTargetId, accessKey, secretKey, this );
			targetStatusPoller.startPolling();
		}
	}
	
	// Called with each update of the target status received by the TargetStatusPoller
	@Override
	public void OnTargetStatusUpdate(TargetState target_state) {
		if (target_state.hasState) {
			
			String status = target_state.getStatus();
			
			System.out.println("Target status is: " + (status != null ? status : "unknown"));
			
			if (target_state.getActiveFlag() == true && "success".equalsIgnoreCase(status)) {
				
				targetStatusPoller.stopPolling();
				
				System.out.println("Target is now in 'success' status");
			}
		}
	}
	
	
	public static void main() throws URISyntaxException, ClientProtocolException, IOException, JSONException {
		PostNewTarget p = new PostNewTarget();
		p.postTargetThenPollStatus();
	}
	
}
