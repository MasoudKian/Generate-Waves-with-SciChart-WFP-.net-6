using SciChart.Charting.Visuals;
using System.Configuration;
using System.Data;
using System.Windows;

namespace sci7
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SciChartSurface.SetRuntimeLicenseKey("i9co/Fbu84GgvU3Nj1sg39k+8yAZrl3u0SWbwnEdV34jDOVDTp2g8n1EOqozp2+f0TbmLSmc8RpDTIotP71Q+XYgODQ84E0dpkgwIFp6LHWA25+4KymISWxAdKpZ5HQ74QMpVfVIzL4uPm6lqwJvsKv0t2g08B/2ECYrCC3irGnJ+8AqvRS0gX0g6faUUNSLGi4patIwiNN6RZxC/agEPDMExfp/paT5fWqYx4AeIifVX+WqFLdzWN0bOcVtaZpjqGRCjBCYkef4Eo6qeQtpgAckPPgO812OywSuNl8LIw8MoOIy8ARFg4HNpy3rtR4JStg72t3SxYQy/Q2kv9zHQXVpTT61uVBrzsc1v7snag7ULlPup1RbVyZXtJhQ5zpTDWfmJJfk1MsvaNOCG1ty8KrD5R2a1qwsSIkxtxP1ineUauZSKPx2lLFuvTQ2S4Ezy7bXsClBFUgVjRg1wwia90BNQqbpew1a34prfah2rg5rff9v6l9cU8jUDaEyMiTdem95jx9e8fbpmlM1IMI8c0xCfT7Unjjcs6Armcrcb9BiRHi7kWJr456JFzUPi6wN3YfUoNk=");
        }
    }

}
