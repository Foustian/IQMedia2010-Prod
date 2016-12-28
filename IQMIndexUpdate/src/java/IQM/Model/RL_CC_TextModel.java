/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package IQM.Model;

import IQM.Core.IQMSession;
import microsoft.sql.Types;
import microsoft.sql.DateTimeOffset;
import java.sql.CallableStatement;
import java.sql.Connection;
import java.sql.Statement;
import java.lang.Class;
import java.sql.DriverManager;

/**
 *
 * @author Administrator
 */

public class RL_CC_TextModel
{
    public int UpdateStatus(String p_StatusMsg,String p_IQ_CC_Keys) throws Exception
    {
        try
        {
            CallableStatement _CallableStatement = null;
            Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
            Connection _Connection = DriverManager.getConnection(IQMSession._IQMConfiguration.getConnectionString());
            _CallableStatement = _Connection.prepareCall("{ call usp_RL_CC_Text_ChangeStatus(?,?) }");
            _CallableStatement.setString("StatusMsg" , p_StatusMsg);
            _CallableStatement.setString("IQ_CC_Keys" , p_IQ_CC_Keys);

           // Statement statement = _Connection.createStatement();
            //ResultSet resultSet = statement.executeQuery("SELECT FirstName FROM Customer");

             int _TotalAffectedRecords= _CallableStatement.executeUpdate();

             _Connection.close();             

             return _TotalAffectedRecords;
        }
        catch(Exception _Exception)
        {
            throw _Exception;
        }
        
    }
}
