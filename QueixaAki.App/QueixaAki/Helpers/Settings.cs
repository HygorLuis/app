using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace QueixaAki.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants

        private const string IdUsuarioToken = "idusuariotoken";
        private static readonly string IdUsuarioTokenDefault = string.Empty;

        private const string ServidorToken = "servidortoken";
        private static readonly string ServidorTokenDefault = string.Empty;

        private const string BancoToken = "bancotoken";
        private static readonly string BancoTokenDefault = string.Empty;

        private const string UsuarioToken = "usuariotoken";
        private static readonly string UsuarioTokenDefault = string.Empty;

        private const string SenhaToken = "senhatoken";
        private static readonly string SenhaTokenDefault = string.Empty;

        #endregion

        public static string IdUsuario
        {
            get => AppSettings.GetValueOrDefault(IdUsuarioToken, IdUsuarioTokenDefault);
            set => AppSettings.AddOrUpdateValue(IdUsuarioToken, value);
        }

        public static string Servidor
        {
            get => AppSettings.GetValueOrDefault(ServidorToken, ServidorTokenDefault);
            set => AppSettings.AddOrUpdateValue(ServidorToken, value);
        }

        public static string Banco
        {
            get => AppSettings.GetValueOrDefault(BancoToken, BancoTokenDefault);
            set => AppSettings.AddOrUpdateValue(BancoToken, value);
        }

        public static string Usuario
        {
            get => AppSettings.GetValueOrDefault(UsuarioToken, UsuarioTokenDefault);
            set => AppSettings.AddOrUpdateValue(UsuarioToken, value);
        }

        public static string Senha
        {
            get => AppSettings.GetValueOrDefault(SenhaToken, SenhaTokenDefault);
            set => AppSettings.AddOrUpdateValue(SenhaToken, value);
        }

        public static void LimparRegistro()
        {
            IdUsuario = "";
            Servidor = "";
            Banco = "";
            Usuario = "";
            Senha = "";
        }
    }
}
