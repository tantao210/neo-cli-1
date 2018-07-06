using Microsoft.Extensions.Configuration;
using Neo.Network;

namespace Neo
{
    internal class Settings
    {
        public PathsSettings Paths { get; }
        public P2PSettings P2P { get; }
        public RPCSettings RPC { get; }
        public UnlockWalletSettings UnlockWallet { get; set; }
        public PassphraseSettings Passphrase { get; }
        public AssetSettings Asset { get; }
        public WalletSettings Wallet { get; }
        public static Settings Default { get; }

        static Settings()
        {
            IConfigurationSection section = new ConfigurationBuilder().AddJsonFile("config.json").Build().GetSection("ApplicationConfiguration");
            Default = new Settings(section);
        }

        public Settings(IConfigurationSection section)
        {
            this.Paths = new PathsSettings(section.GetSection("Paths"));
            this.P2P = new P2PSettings(section.GetSection("P2P"));
            this.RPC = new RPCSettings(section.GetSection("RPC"));
            this.UnlockWallet = new UnlockWalletSettings(section.GetSection("UnlockWallet"));
            Passphrase = new PassphraseSettings(section.GetSection("Passphrase"));
            Asset = new AssetSettings(section.GetSection("Asset"));
            Wallet = new WalletSettings(section.GetSection("Wallet"));
        }
    }

    internal class PathsSettings
    {
        public string Chain { get; }
        public string ApplicationLogs { get; }

        public PathsSettings(IConfigurationSection section)
        {
            this.Chain = string.Format(section.GetSection("Chain").Value, Message.Magic.ToString("X8"));
            this.ApplicationLogs = string.Format(section.GetSection("ApplicationLogs").Value, Message.Magic.ToString("X8"));
        }
    }

    internal class P2PSettings
    {
        public ushort Port { get; }
        public ushort WsPort { get; }

        public P2PSettings(IConfigurationSection section)
        {
            this.Port = ushort.Parse(section.GetSection("Port").Value);
            this.WsPort = ushort.Parse(section.GetSection("WsPort").Value);
        }
    }

    internal class RPCSettings
    {
        public ushort Port { get; }
        public string SslCert { get; }
        public string SslCertPassword { get; }

        public RPCSettings(IConfigurationSection section)
        {
            this.Port = ushort.Parse(section.GetSection("Port").Value);
            this.SslCert = section.GetSection("SslCert").Value;
            this.SslCertPassword = section.GetSection("SslCertPassword").Value;
        }
    }

    public class UnlockWalletSettings
    {
        public string Path { get; }
        public string Password { get; }
        public bool StartConsensus { get; }
        public bool IsActive { get; }

        public UnlockWalletSettings(IConfigurationSection section)
        {
            if (section.Value != null)
            {
                this.Path = section.GetSection("WalletPath").Value;
                this.Password = section.GetSection("WalletPassword").Value;
                this.StartConsensus = bool.Parse(section.GetSection("StartConsensus").Value);
                this.IsActive = bool.Parse(section.GetSection("IsActive").Value);
            }
        }
    }

    internal class PassphraseSettings
    {
        public string Passphrase { get; }

        public PassphraseSettings(IConfigurationSection section)
        {
            this.Passphrase = section.GetSection("passphrase").Value;
            if (string.IsNullOrEmpty(this.Passphrase))
            {
                this.Passphrase = "tiger";
            }
        }
    }

    internal class AssetSettings
    {
        public string GASAsset { get; }
        public string NEOAsset { get; }
        public AssetSettings(IConfigurationSection section)
        {
            this.GASAsset = section.GetSection("gasasset").Value;
            this.NEOAsset = section.GetSection("neoasset").Value;
            if (string.IsNullOrEmpty(GASAsset))
            {
                this.GASAsset = "602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7";
            }
            if (string.IsNullOrEmpty(NEOAsset))
            {
                this.NEOAsset = "c56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b";
            }
        }
    }

    internal class WalletSettings
    {
        public string Path { get; }

        public WalletSettings(IConfigurationSection section)
        {
            Path = section.GetSection("path").Value;
            if (string.IsNullOrEmpty(Path))
            {
                Path = "wallet/wallet.json";
            }
        }
    }
}
