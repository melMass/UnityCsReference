// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using UnityEngine.UIElements;

namespace UnityEditor.PackageManager.UI
{
    internal class PackageVersionItem : VisualElement, ISelectableItem
    {
        public IPackage package { get; set; }
        public IPackageVersion version { get; set; }

        public PackageVersionItem(IPackage package, IPackageVersion version)
        {
            var root = Resources.GetTemplate("PackageVersionItem.uxml");
            Add(root);
            cache = new VisualElementCache(root);
            this.package = package;
            this.version = version;
            RefreshLabel();
            this.OnLeftClick(() => PageManager.instance.SetSelected(package, version, true));
        }

        public IPackageVersion targetVersion { get { return version; } }
        public VisualElement element { get { return this; } }

        private void RefreshLabel()
        {
            versionLabel.text = version.version?.ToString() ?? version.versionString;
            var primary = package.versions.primary;
            Label stateLabel = null;
            if (version == primary)
            {
                if (version.isInstalled)
                    stateLabel = new Label(L10n.Tr("Currently Installed"));
                else if (version.HasTag(PackageTag.Downloadable) && version.isAvailableOnDisk)
                    stateLabel = new Label(L10n.Tr("Currently Downloaded"));
            }
            else if (version == package.versions.recommended)
                stateLabel = new Label(L10n.Tr("Update Available"));
            if (stateLabel != null)
            {
                stateLabel.tooltip = stateLabel.text;
                stateContainer.Add(stateLabel);
            }

            var tagLabel = PackageTagLabel.CreateTagLabel(version, true);
            if (tagLabel != null)
                stateContainer.Add(tagLabel);
        }

        private VisualElementCache cache { get; set; }
        private Label versionLabel { get { return cache.Get<Label>("versionLabel"); } }
        private VisualElement stateContainer { get { return cache.Get<VisualElement>("stateContainer"); } }
    }
}