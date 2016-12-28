/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package IQM.Core;

/**
 *
 * @author Administrator
 */
public class Person {
    /*
private String firstName;
private String lastName;
private test testobj;

public class test { private String testAddr; public String gettestAddr(){return testAddr;} public void settestAddr(String str){ testAddr=str;} }

public String getFirstName() { return firstName; }
public String getLastName() { return lastName; }
public test getTestobj(){return testobj;}

public void setFirstName(String str) { firstName = str; }
public void setLastName(String str) { lastName = str; }
public void setTestobj(test _testobj){testobj=_testobj;}*/

     private String log_file_location;

    public void setlog_file_location(String p_log_file_location)
    {
        log_file_location=p_log_file_location;
    }

    public String getlog_file_location()
    {
        return log_file_location;
    }
}
