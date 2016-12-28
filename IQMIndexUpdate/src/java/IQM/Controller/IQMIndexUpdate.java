/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package IQM.Controller;

import java.io.IOException;
import java.io.PrintWriter;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.FileInputStream;
import com.thoughtworks.xstream.*;
import com.thoughtworks.xstream.io.xml.DomDriver;
import com.thoughtworks.xstream.io.xml.XmlFriendlyReplacer;
import java.io.DataInputStream;
import java.io.BufferedReader;
import java.io.InputStreamReader;

import IQM.Core.*;
import java.util.Date;


/**
 *
 * @author Administrator
 */
public class IQMIndexUpdate extends HttpServlet
{
   
    /** 
     * Processes requests for both HTTP <code>GET</code> and <code>POST</code> methods.
     * @param request servlet request
     * @param response servlet response
     * @throws ServletException if a servlet-specific error occurs
     * @throws IOException if an I/O error occurs
     */
    protected void processRequest(HttpServletRequest request, HttpServletResponse response)
    throws ServletException, IOException
    {
        response.setContentType("text/xml;charset=UTF-8");
        PrintWriter out = response.getWriter();
        try
        {
            

           String _ParamValue=request.getParameter("op");
           XStream _XStream = new XStream(new DomDriver());

           IQMConfiguration _IQMConfiguration = new IQMConfiguration();
           _XStream.alias("IQMConfiguration", IQMConfiguration.class);
           FileInputStream _FileInputStream = new FileInputStream(getServletContext().getInitParameter("IQMIndexUpdateStatusConfFile"));                        
           _XStream.fromXML(_FileInputStream, _IQMConfiguration);

           IQMSession._IQMConfiguration=_IQMConfiguration;

           IQMLog.LogInfo("IQMIndexUpdate Started");

            _FileInputStream = new FileInputStream(_IQMConfiguration.getstatus_file_location());

            IQMIndexStatus _IQMIndexStatus=new IQMIndexStatus();
            _XStream.alias("IQMStatus", IQMIndexStatus.class);

             _XStream.fromXML(_FileInputStream,_IQMIndexStatus);

             IQMLog.LogInfo("Check method");

            if (_ParamValue.toLowerCase().equals("status"))
            {
                IQMLog.LogInfo("Status called");

                 _FileInputStream = new FileInputStream(_IQMConfiguration.getstatus_file_location());

                DataInputStream _DataInputStream = new DataInputStream(_FileInputStream);
                BufferedReader _BufferedReader = new BufferedReader(new InputStreamReader(_DataInputStream));

                String strLine;
                
                while ((strLine = _BufferedReader.readLine()) != null)
                {                  
                    out.write(strLine);
                }

                IQMLog.LogInfo("IQMIndexUpdate Finished");
            }
            else if (_ParamValue.toLowerCase().equals("index"))
            {
                IQMLog.LogInfo("Index called");

                if (_IQMIndexStatus.getIndexing_Started()==true)
                {
                    IQMLog.LogInfo("Indexing already started");

                    _FileInputStream = new FileInputStream(_IQMConfiguration.getstatus_file_location());

                    DataInputStream _DataInputStream = new DataInputStream(_FileInputStream);
                    BufferedReader _BufferedReader = new BufferedReader(new InputStreamReader(_DataInputStream));

                    String strLine;

                    while ((strLine = _BufferedReader.readLine()) != null)
                    {
                        out.write(strLine);
                    }

                    IQMLog.LogInfo("IQMIndexUpdate Finished");
                }
                else
                {
                    IQMLog.LogInfo("Indexing started");

                    _IQMIndexStatus.setIndexing_Started(true);
                    _IQMIndexStatus.setIndex_Start_time(new Date().toString());

                    _IQMIndexStatus.setCore0Commit(new IQMIndexStatus.Core0Commit());
                    _IQMIndexStatus.setCore1Commit(new IQMIndexStatus.Core1Commit());
                    _IQMIndexStatus.setCore1Ingest(new IQMIndexStatus.Core1Ingest());
                    _IQMIndexStatus.setCore2Commit(new IQMIndexStatus.Core2Commit());
                    _IQMIndexStatus.setCore2Optimize(new IQMIndexStatus.Core2Optimize());
                    _IQMIndexStatus.setCoreDelete(new IQMIndexStatus.CoreDelete());                    
                    _IQMIndexStatus.setCoreMerge(new IQMIndexStatus.CoreMerge());
                    _IQMIndexStatus.setCoreSwap(new IQMIndexStatus.CoreSwap());
                    _IQMIndexStatus.setSQLDB_Index_Update(new IQMIndexStatus.SQLDB_Index_Update());

                    out.write("<IQMIndex_Update_response><reponse_txt>Offline Indexing Started</reponse_txt><response_time>"+new Date().toString()+"</response_time></IQMIndex_Update_response>");

                    XmlFriendlyReplacer replacer = new XmlFriendlyReplacer("ddd", "_");
                    _XStream = new XStream(new DomDriver("UTF-8", replacer));

                     FileOutputStream _FileOutputStream = new FileOutputStream(_IQMConfiguration.getstatus_file_location());

                     _XStream.toXML(_IQMIndexStatus, _FileOutputStream);

                     IQMSession._IQMIndexStatus=_IQMIndexStatus;

                     IQMIndexProcessor _IQMIndexProcessor=new IQMIndexProcessor();
                     new Thread(_IQMIndexProcessor).start();

                    //_IQMIndexStatus.setIndex_Start_time(Date.);
                /*}*/
            }
        }
        else
        {
           IQMLog.LogInfo("IQMIndexUpdate Finished");
           out.write("<IQMIndex_Update_response><reponse_txt>Invalid Input</reponse_txt><response_time>"+new Date().toString()+"</response_time></IQMIndex_Update_response>");
        }
       /* catch(Exception ex)
        {
            System.out.println("<html><body>"+ex.getMessage()+"</body></html>");
        }*/
        
    }
        catch(Exception _Exception)
        {
             try
             {
                IQMLog.LogInfo(_Exception.getMessage());
             }
             catch(Exception _InnerException)
             {
                _InnerException.printStackTrace();
             }
            _Exception.printStackTrace();
        }
        finally
        {
            out.close();
        }
   }

    // <editor-fold defaultstate="collapsed" desc="HttpServlet methods. Click on the + sign on the left to edit the code.">
    /** 
     * Handles the HTTP <code>GET</code> method.
     * @param request servlet request
     * @param response servlet response
     * @throws ServletException if a servlet-specific error occurs
     * @throws IOException if an I/O error occurs
     */
    @Override
    protected void doGet(HttpServletRequest request, HttpServletResponse response)
    throws ServletException, IOException {
        processRequest(request, response);
    } 

    /** 
     * Handles the HTTP <code>POST</code> method.
     * @param request servlet request
     * @param response servlet response
     * @throws ServletException if a servlet-specific error occurs
     * @throws IOException if an I/O error occurs
     */
    @Override
    protected void doPost(HttpServletRequest request, HttpServletResponse response)
    throws ServletException, IOException {
        processRequest(request, response);
    }

    /** 
     * Returns a short description of the servlet.
     * @return a String containing servlet description
     */
    @Override
    public String getServletInfo() {
        return "Short description";
    }// </editor-fold>

}
