/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package IQM.Core;

import org.apache.log4j.Logger;
import org.apache.log4j.BasicConfigurator;
import org.apache.log4j.Level;
import org.apache.log4j.SimpleLayout;
import org.apache.log4j.FileAppender;
import org.apache.log4j.PatternLayout;

import java.io.File;
import java.util.Calendar;
import java.util.Date;

/**
 *
 * @author Administrator
 */
public class IQMLog
{
    private static final Logger _Logger = Logger.getLogger(IQMLog.class);

    private static final String pattern =  "%d{ISO8601}"+" INFO %m %n %n";
    private static final PatternLayout layout = new PatternLayout(pattern);
    private static FileAppender appender;

    static
    {
        try
        {            
            appender =new FileAppender(layout, getFile(), false);
            _Logger.addAppender(appender);
        }
        catch(Exception _Exception)
        {
           _Exception.printStackTrace();    
        }
    }   

    public static String getFile() throws Exception
    {
        try
        {            
            Calendar _Calendar=Calendar.getInstance();

            File _File=new File(IQMSession._IQMConfiguration.getlog_file_location()+"iQMIndexUpdate_log_"+String.valueOf(_Calendar.get(Calendar.YEAR))+String.valueOf(_Calendar.get(Calendar.MONTH)+1)+ String.valueOf(_Calendar.get(Calendar.DAY_OF_MONTH))+".txt");

            if (!_File.exists())
            {
                _File.createNewFile();
            }           

            return _File.getAbsolutePath();
        }
        catch(Exception _Exception)
        {
            _Exception.printStackTrace();
            throw _Exception;
        }
    }

    public static void LogInfo(String p_Msg) throws Exception
    {
        try
        {     
            _Logger.setLevel(Level.INFO);
            
            _Logger.info(p_Msg);            

        }
        catch (Exception _Exception)
        {
            _Exception.printStackTrace();
            throw _Exception;
        }
    }

    public static void LogDebug(String p_Msg) throws Exception
    {
        try
        {
            _Logger.setLevel(Level.DEBUG);

            _Logger.debug(p_Msg);
        }
        catch (Exception _Exception)
        {
            _Exception.printStackTrace();
            throw _Exception;
        }
    }

     public static void LogWarn(String p_Msg) throws Exception
    {
         try
         {
            _Logger.setLevel(Level.WARN);

            _Logger.warn(p_Msg);
         }
         catch (Exception _Exception)
        {         
            _Exception.printStackTrace();
            throw _Exception;
        }
     }

     public  static void LogError(String p_Msg) throws Exception
    {
         try
         {                       
            _Logger.setLevel(Level.ERROR);

            _Logger.error(p_Msg);
         }
         catch (Exception _Exception)
        {         
            _Exception.printStackTrace();
            throw _Exception;
        }
     }

      public void LogFatal(String p_Msg) throws Exception
    {
         try
         {            
            _Logger.setLevel(Level.FATAL);

            _Logger.fatal(p_Msg);
         }
         catch (Exception _Exception)
        {            
            _Exception.printStackTrace();
            throw _Exception;
        }
     }
}
