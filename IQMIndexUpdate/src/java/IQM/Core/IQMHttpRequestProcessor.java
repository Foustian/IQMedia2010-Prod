/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package IQM.Core;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.URL;
import java.net.URLConnection;
import java.net.HttpURLConnection;
import java.net.ProtocolException;
import java.io.OutputStream;
import java.io.Writer;
import java.io.Reader;
import java.io.OutputStreamWriter;
import java.io.InputStream;
import java.io.IOException;

/**
 *
 * @author Administrator
 */
public class IQMHttpRequestProcessor
{
    public static String MakeGetRequest(String p_HttpURL) throws Exception
    {
        String _Response=null;

        try
        {
            URL _URL = new URL(p_HttpURL);
            URLConnection _URLConnection = _URL.openConnection ();

            BufferedReader _BufferedReader = new BufferedReader(new InputStreamReader(_URLConnection.getInputStream()));
            StringBuffer _StringBuffer = new StringBuffer();
            String _OutputLine;

            while ((_OutputLine = _BufferedReader.readLine()) != null)
            {
                _StringBuffer.append(_OutputLine);
            }

            _BufferedReader.close();
            _Response= _StringBuffer.toString();

        }
        catch (Exception e)
        {
           throw e;
        }

        return _Response;
    }

    public static String MakePostRequest(String data, URL endpoint) throws Exception
    {
        try
        {

            HttpURLConnection urlc = null;
            String _Response="";

            try
            {
                urlc = (HttpURLConnection) endpoint.openConnection();
            try
            {
                urlc.setRequestMethod("POST");
            }
            catch (ProtocolException e)
            {
                throw new Exception("Shouldn't happen: HttpURLConnection doesn't support POST??", e);
            }

            urlc.setDoOutput(true);
            urlc.setDoInput(true);
            urlc.setUseCaches(false);
            urlc.setAllowUserInteraction(false);
           // urlc.setRequestProperty("Content-type", "text/xml; charset=" + "UTF-8");

            OutputStream out = urlc.getOutputStream();

            try
            {
                out.write(data.getBytes("UTF-8"));
                /*Writer writer = new OutputStreamWriter(out, "UTF-8");
                pipe(data, writer);
                writer.close();*/
            }
            catch (IOException e)
            {
                throw new Exception("IOException while posting data", e);
            }
            finally
            {
                if (out != null)
                {
                    out.close();
                }
            }

            InputStream in = urlc.getInputStream();

            try
            {
                /*Reader reader = new InputStreamReader(in);
                pipe(reader, output);
                reader.close();           */

                BufferedReader _BufferedReader = new BufferedReader(new InputStreamReader(urlc.getInputStream()));
                StringBuffer _StringBuffer = new StringBuffer();
                String _OutputLine;

                while ((_OutputLine = _BufferedReader.readLine()) != null)
                {
                    _StringBuffer.append(_OutputLine);
                }

                _BufferedReader.close();
                _Response= _StringBuffer.toString();

            }
            catch (IOException e)
            {
                throw new Exception("IOException while reading response", e);
            }
            finally
            {
                if (in != null)
                {
                    in.close();
                }
            }

            }
            catch (IOException e)
            {
                throw new Exception("Connection error (is server running at " + endpoint + " ?): " + e);
            }
            finally
            {
                if (urlc != null)
                {
                    urlc.disconnect();
                }
            }

            return _Response;
        }
        catch(Exception _Exception)
        {
            throw _Exception;
        }
    }

    private static void pipe(Reader reader, Writer writer) throws IOException
    {
        char[] buf = new char[1024];
        int read = 0;
        while ((read = reader.read(buf)) >= 0)
        {
            writer.write(buf, 0, read);
        }
        writer.flush();

    }
}
