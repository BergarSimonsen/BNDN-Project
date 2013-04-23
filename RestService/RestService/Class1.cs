using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestService
{
    class Class1
    {
        public static void Main(string[] args)
        {
            DatabaseConnection database = DatabaseConnection.GetInstance;
            //Rating r1 = database.getRating("4", "18");
            //Console.WriteLine(r1.id);
            //Console.WriteLine(r1.userId);
            //Console.WriteLine(r1.mediaId);
            //Console.WriteLine(r1.rating);
            //Console.WriteLine(r1.comment);
            //Console.WriteLine(r1.commentTitle);
            Console.ReadKey();
        }

    }
}
