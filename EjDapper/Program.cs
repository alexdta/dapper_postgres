using System;
using Dapper;
using Npgsql;
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace EjDapper
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string connection =
                "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=ej_dapper;";

                using (var db = new NpgsqlConnection(connection))
                {
                    var insert = "INSERT INTO persona(nombre, edad) VALUES (@nombre, @edad);";

                    //Insert con valores
                    db.Execute(insert, new { nombre = "Pepe", edad = 60 });

                    //Insert con Objeto
                    var Juana = new Persona() { nombre = "Juana", edad = 100 };
                    db.Execute(insert, Juana);

                    //Insert varias personas
                    List<Persona> lista = new List<Persona>() 
                    {
                        new Persona() { nombre = "José", edad = 27 },
                        new Persona() { nombre = "Priscilla", edad = 38 }
                    };
                    db.Execute(insert, lista);

                    //Update con objeto
                    var selecJuana = "SELECT id, nombre, edad FROM persona WHERE nombre = 'Juana2'";
                    Juana = db.QueryFirstOrDefault<Persona>(selecJuana);

                    if (Juana != null)
                    {
                        var update = "UPDATE persona SET edad = @edad WHERE id = @id;";
                        Juana.edad = 75;//Cambio el dato
                        db.Execute(update, Juana);
                    }

                    //Listado de todas las personas
                    var select = "SELECT id, nombre, edad FROM persona ORDER BY 1 ASC;";

                    var lstPersonas = db.Query<Persona>(select);

                    foreach (var persona in lstPersonas)
                    {
                        Console.WriteLine($"Nombre: {persona.nombre}, Edad: {persona.edad}");
                    }

                    Console.WriteLine();
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine();
                    Console.ReadKey();

                    //Delete
                    var delete = "DELETE FROM persona WHERE id > 4";
                    db.Execute(delete);

                    lstPersonas = db.Query<Persona>(select);

                    foreach (var persona in lstPersonas)
                    {
                        Console.WriteLine($"Nombre: {persona.nombre}, Edad: {persona.edad}");
                    }
                }

                Console.ReadKey();
            }
            catch (Exception lExcp)
            {
                Console.WriteLine(lExcp.Message);
                Console.ReadKey();
            }

        }
    }
}
/* 
CREATE TABLE persona (
  id 		serial,
  nombre 	CHARACTER VARYING(64) NOT NULL,
  edad 		INTEGER NOT NULL,
		PRIMARY KEY (id)
);

INSERT INTO persona(nombre, edad) VALUES ('Juan', 18);
INSERT INTO persona(nombre, edad) VALUES ('Pedro', 32);
INSERT INTO persona(nombre, edad) VALUES ('Maria', 21);
INSERT INTO persona(nombre, edad) VALUES ('Marcela', 40);
 */