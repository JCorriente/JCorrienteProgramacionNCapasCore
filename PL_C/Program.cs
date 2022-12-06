// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
//Console.ReadKey();
ReadFile();
Console.ReadKey();

static void ReadFile()
{
    string file = @"C:\Users\digis\Documents\Jaime Corriente Romero\JCorrienteProgramacionNCapasCore\LayoutUsuarios.txt";
    if (File.Exists(file))
    {
        StreamReader Textfile = new StreamReader(file);
        string line;
        line = Textfile.ReadLine();

        ML.Result resultErrores = new ML.Result();
        resultErrores.Objects = new List<object>();

        while ((line = Textfile.ReadLine()) != null)
        {
            string  [] lines = line.Split('|');

           ML.Usuario usuario = new ML.Usuario(); 
           
            usuario.Nombre = lines[0];
            usuario.ApellidoPaterno = lines[1];
            usuario.ApellidoMaterno = lines[2];
            usuario.FechaNacimiento = lines[3];
            usuario.Genero  = lines[4];
            usuario.Email   = lines[5];
            usuario.Password = lines[6];
            usuario.Telefono = lines[7];
            usuario.Celular = lines[8];
            usuario.CURP  = lines[9];
            usuario.UserName = lines[10];
            
            usuario.Rol = new ML.Rol();
            usuario.Rol.IdRol = byte.Parse(lines[11]);
            usuario.Imagen = null;

            usuario.Direccion = new ML.Direccion();
            usuario.Direccion.Calle = lines[12];
            usuario.Direccion.NumeroExterior = lines[13];
            usuario.Direccion.NumeroInterior = lines[14];

            usuario.Direccion.Colonia = new ML.Colonia();
            usuario.Direccion.Colonia.IdColonia = int.Parse(lines[15]);

            ML.Result result = BL.Usuario.Add(usuario);

            if (!result.Correct)
            {
                resultErrores.Objects.Add(
                    "No se inserto el Nombre correctamente" + usuario.Nombre +
                    "No se inserto el Apellido Paterno correctamente" + usuario.ApellidoPaterno +
                    "No se inserto el Apellido Materno correctamente" + usuario.ApellidoMaterno +
                    "No se inserto la Fecha de nacimiento correctamente" + usuario.FechaNacimiento +
                    "No se inserto el Genero correctamente" + usuario.Genero +
                    "No se inserto el Email de nacimiento correctamente" + usuario.Email +
                    "No se inserto el Password correctamente" + usuario.Password +
                    "No se inserto el Telefono de nacimiento correctamente" + usuario.Telefono +
                    "No se inserto el Celular de nacimiento correctamente" + usuario.Celular +
                    "No se inserto el CURP de nacimiento correctamente" + usuario.CURP +
                    "No se inserto la Imagen correctamente" + usuario.Imagen +
                    "No se inserto el IdRol correctamente" + usuario.Rol.IdRol +
                    "No se inserto la Calle de nacimiento correctamente" + usuario.Direccion.Calle +
                    "No se inserto el NumeroExterior de nacimiento correctamente" + usuario.Direccion.NumeroExterior +
                      "No se inserto el NumeroInterior de nacimiento correctamente" + usuario.Direccion.NumeroInterior +
                    "No se inserto la Colonia de nacimiento correctamente" + usuario.Direccion.Colonia +
                    "No se inserto el IdColonia correctamente" + usuario.Direccion.Colonia.IdColonia);
            }
            if (result.Correct)
            {
                result.Message = "Usuario Agregado correctamente";
            }
            else
            {
                result.Message = "Usuario no agregado";
            }
        }
        if (resultErrores.Objects != null)
        {
            TextWriter tx = new StreamWriter(@"C:\Users\digis\Documents\Jaime Corriente Romero\JCorrienteProgramacionNCapasCore\LayoutErrores.txt");
            foreach (string error in resultErrores.Objects)
            {
                tx.WriteLine(error);
            }
            tx.Close();
        }
        
    
    }
}