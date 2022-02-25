"""
Programmers: Amritpal Singh, Dhruvanshi Ghiya
Project: Logging Service
File Name: TestClient.py
Date: 24 Feburary,2022
Description: This is the client file. 
"""



import socket
import sys


noOfCommandLineArguments = len(sys.argv)
if(noOfCommandLineArguments != 3 ):
    print("Error... Usage: Python TestClient.py Server_IP_Address Port")
    exit()


hostIPAddress = str(sys.argv[1])
targetPort = int(sys.argv[2])


try:
    # create a socket connection
    client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    # let the client connect
    client.connect((hostIPAddress, targetPort))



    """
    The standard ranking of logging levels
    is as follows: ALL < TRACE < DEBUG < INFO < WARN < ERROR < FATAL < OFF.
    
    • OFF: This log level does not log anything. This OFF level is used to turn off logging and is the greatest
    possible rank. With this log level, nothing gets logged at all.
    
    • FATAL: FATAL means that the application is about to stop a serious problem or corruption from
    happening. The FATAL level of logging shows that the application’s situation is catastrophic, such that an
    important function is not working. For example, you can use FATAL log level if the application is unable to
    connect to the data store.
    
    • ERROR: Unlike the FATAL logging level, error does not mean your application is aborting. Instead, there is
    just an inability to access a service or a file. This ERROR shows a failure of something important in your
    application. This log level is used when a severe issue is stopping functions within the application from
    operating efficiently. Most of the time, the application will continue to run, but eventually, it will need to
    be addressed.
    
    • WARN: The WARN log level is used when you have detected an unexpected application problem. This
    means you are not quite sure whether the problem will recur or remain. You may not notice any harm to
    your application at this point. This issue is usually a situation that stops specific processes from running.
    Yet it does not mean that the application has been harmed. In fact, the code should continue to work as
    usual. You should eventually check these warnings just in case the problem reoccurs.
    
    • INFO: INFO messages are like the normal behavior of applications. They state what happened. For
    example, if a particular service stopped or started or you added something to the database. These entries
    are nothing to worry about during usual operations. The information logged using the INFO log is usually
    informative, and it does not necessarily require you to follow up on it.
    
    • DEBUG: With DEBUG, you are giving diagnostic information in a detailed manner. It is verbose and has
    more information than you would need when using the application. DEBUG logging level is used to fetch
    information needed to diagnose, troubleshoot, or test an application. This ensures a smooth running
    application.
    
    • TRACE: The TRACE log level captures all the details about the behavior of the application. It is mostly
    diagnostic and is more granular and finer than DEBUG log level. This log level is used in situations where
    you need to see what happened in your application or what happened in the third-party libraries used.
    You can use the TRACE log level to query parameters in the code or interpret the algorithm’s steps.
    
    """


    logLevel = 0

    # loop until user choose to
    while(logLevel != -1):

        print("Log Levels:\n", "0. OFF\n", "1. FATAL\n", "2. ERROR\n", "3. WARN\n", "4. INFO\n", "5. DEBUG\n", "6. TRACE\n")


        # ask logging level from client
        logLevel = 0
        logLevel = input("Choose your log Level[0-6] or -1 to exit: ")
        logLevel = int(logLevel)

        if (logLevel == -1):
            break

        if(logLevel < 0 and logLevel >6):
            print("Error: Please enter your log level within given Range only")
            continue

        logLevel = str(logLevel)

        Message = input("Please enter the message to be sent: ")

        # message to be sent
        clientMessage = logLevel + "#" + Message

        #sending the message
        client.sendall(clientMessage.encode())

        # receive the response from server
        response = client.recv(4096)

        # print server response
        if (response.decode() == "SUCCESS!!!"):
            print(response.decode())
        else:
            print("Something wrong with Connection or Message sent!!")

except:
  print("Cannot connect to Server....Something went wrong")