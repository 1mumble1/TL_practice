namespace Dictionary;

public class Program
{
    private static void Main( string[] args )
    {
        string filePath = string.Empty;
        Dictionary dictionary = new();

        Console.WriteLine( "This console application was created as dictionary." );
        filePath = TryInitializeDictionary( dictionary );

        while ( true )
        {
            Console.Clear();
            Console.WriteLine( "Menu: \n" +
                "1 - Translate word\n" +
                "2 - Add new translation\n" +
                "0 - Exit" );
            ConsoleKeyInfo userInput = Console.ReadKey();
            Console.WriteLine();
            switch ( userInput.Key )
            {
                case ConsoleKey.D1:
                    Translate( dictionary );

                    break;

                case ConsoleKey.D2:
                    AddNewTranslation( dictionary );

                    break;

                case ConsoleKey.D0:
                    filePath = SaveDictionary( dictionary );
                    Console.WriteLine( $"Dictionary successfully saved! Goodbye" );

                    return;

                default:
                    Console.WriteLine( "Invalid input" );
                    Console.ReadKey();

                    break;
            }
        }
    }

    private static void Translate( Dictionary dictionary )
    {
        Console.Write( "Enter word for translating: " );
        string word = GetString();
        List<string>? translations = GetTranslation( dictionary, word );

        if ( translations is null )
        {
            Console.WriteLine( $"Word '{word}' not found in dictionary. Do you want to add this word?" );
            Console.WriteLine( "Press Enter to confirm, any other button to discard" );
            ConsoleKeyInfo userInputConfirmation = Console.ReadKey();
            if ( userInputConfirmation.Key != ConsoleKey.Enter )
            {
                return;
            }

            AddNewTranslationForWord( word, dictionary );
        }
        else
        {
            Console.WriteLine( $"Translation: {string.Join( ", ", translations )}" );
            Console.ReadKey();
        }
    }

    private static string TryInitializeDictionary( Dictionary dictionary )
    {
        Console.WriteLine( "If you want initialize dictionary, press Enter, else click any button: " );

        ConsoleKeyInfo userInputConfirmation = Console.ReadKey();
        if ( userInputConfirmation.Key != ConsoleKey.Enter )
        {
            return string.Empty;
        }

        Console.Write( "Enter file path: " );
        string path = GetString();
        dictionary.Initialize( path );

        return path;
    }

    private static string SaveDictionary( Dictionary dictionary )
    {
        Console.Write( "Enter file path: " );
        string path = GetString();

        dictionary.SaveToFile( path );
        return path;
    }

    private static void AddNewTranslation( Dictionary dictionary )
    {
        Console.Write( "Type word you want to add: " );
        string word = GetString();

        Console.Write( $"Type translation of '{word}': " );
        string translation = GetString();

        AddNewPair( word, translation, dictionary );
    }

    private static void AddNewPair( string word, string translation, Dictionary dictionary )
    {
        if ( dictionary.AddNewWord( word, translation ) )
        {
            Console.WriteLine( $"Pair '{word}' - '{translation}' successfully added!" );
        }
        else
        {
            Console.WriteLine( $"Failed to add pair  '{word}' - '{translation}'" );
        }
        Console.ReadKey();
    }

    private static List<string>? GetTranslation( Dictionary dictionary, string word )
    {
        if ( dictionary.IsEmpty() )
        {
            return null;
        }

        return dictionary.FindWord( word );
    }

    private static void AddNewTranslationForWord( string word, Dictionary dictionary )
    {
        Console.Write( $"Type translation of '{word}': " );
        string translation = GetString();

        AddNewPair( word, translation, dictionary );
    }

    private static string GetString()
    {
        string? str = Console.ReadLine();
        while ( string.IsNullOrWhiteSpace( str ) )
        {
            Console.WriteLine( "This parameter cannot be empty. Try again" );
            str = Console.ReadLine();
        }

        return str;
    }
}