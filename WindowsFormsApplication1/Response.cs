using System;
using System.IO;
using System.Net;

namespace WindowsFormsApplication1
{
    public class Response
    {
        public void HandleResponse(HttpWebRequest WebReq)
        {
            HttpWebResponse WebResp = (HttpWebResponse) WebReq.GetResponse();
            //Let's show some information about the response
            Console.WriteLine(WebResp.StatusCode);
            Console.WriteLine(WebResp.Server);
            Console.WriteLine(WebResp.ResponseUri);

            //Now, we read the response (the string), and output it.
            Stream Answer = WebResp.GetResponseStream();
            StreamReader readStream = new StreamReader(Answer);

            Console.WriteLine("\nResponse stream received");
            Char[] read = new Char[256];

            // Read 256 charcters at a time.     
            int count = readStream.Read(read, 0, 256);
            Console.WriteLine("HTML...\r\n");

            while (count > 0)
            {
                // Dump the 256 characters on a string and display the string onto the console.
                String str = new String(read, 0, count);
                Console.Write(str);
                count = readStream.Read(read, 0, 256);
            }

            Console.WriteLine("");
            // Release the resources of stream object.
            readStream.Close();
            //Console.WriteLine(_Answer.ReadToEnd());

            //Congratulations, with these two functions in basic form, you just learned
            //the two basic forms of web surfing
            //This proves how easy it can be.}
        }

    }
}
