using System;
using System.Linq;
using BeatMapInfo;
using Microsoft.Xna.Framework;
using RocketUI;
using RocketUI.Layout;
using TruSaber.Models;
using TruSaber.Scenes;

namespace TruSaber.Graphics.Gui
{
    public class LevelInfo : MultiStackContainer
    {
        private BeatLevel _level;

        public BeatLevel Level
        {
            get => _level;
            set
            {
                _level = value;
                UpdateContent();
            }
        }

        private ButtonGroup            _difficultySelect;
        private ButtonGroup            _characteristicSelect;
        private TextElement            _title;
        private TextElement            _author;
        private Image                  _albumImage;
        private Button _playButton;

        public Characteristic? SelectedCharacteristic => _characteristicSelect.CheckedControl?.Tag as Characteristic?;
        public Difficulty?     SelectedDifficulty     => _difficultySelect.CheckedControl?.Tag as Difficulty?;
        
        public LevelInfo()
        {
            _difficultySelect = new ButtonGroup();
            _title = new TextElement();
            _author = new TextElement();
            _albumImage = new Image(GuiTextures.PanelGlass);
            _difficultySelect = new ButtonGroup();
            _difficultySelect.CheckedControlChanged += DifficultySelectOnCheckedControlChanged;
            _characteristicSelect = new ButtonGroup();
            _characteristicSelect.CheckedControlChanged += CharacteristicSelectOnCheckedControlChanged;
            _playButton = new Button("PLAY", () => OnPlayClicked());

            AddRow(_albumImage, _title);
            AddRow(_author);
            AddRow(_characteristicSelect);
            AddRow(_difficultySelect);
            AddRow(_playButton);
        }

        private void DifficultySelectOnCheckedControlChanged(object? sender, GuiElementEventArgs e)
        {
            ValidatePlayButton();
        }

        private void OnPlayClicked()
        {
            if (SelectedCharacteristic.HasValue && SelectedDifficulty.HasValue)
            {
                TruSaberGame.Instance.SceneManager.PushScene<PlayLevelScene>(new PlayLevelScene(_level,
                    SelectedCharacteristic.Value, SelectedDifficulty.Value));
            }
        }

        private void UpdateContent()
        {
            _title.Text = _level.MapInfo.SongName;
            _author.Text = _level.MapInfo.SongAuthorName;
            _albumImage.Background = (GuiTexture2D) _level.CoverImagePath;
            
            _difficultySelect.ClearChildren();
            _characteristicSelect.ClearChildren();

            var characteristics = _level.AvailableCharacteristics;
            foreach (var characteristic in characteristics)
            {
                _characteristicSelect.AddChild(new ToggleButton(characteristic.ToString())
                {
                    Tag = characteristic
                });
            }
        }
        
        private void CharacteristicSelectOnCheckedControlChanged(object? sender, GuiElementEventArgs e)
        {
            _difficultySelect.ClearChildren();
            if (e.Element == null)
                return;
            
            if (_level.AvailableDifficulties.TryGetValue((Characteristic)e.Element.Tag, out var difficulties))
            {
                foreach (var difficulty in difficulties)
                {
                    _difficultySelect.AddChild(new ToggleButton(difficulty.ToString())
                    {
                        Tag = difficulty
                    });
                }
            }
            
            ValidatePlayButton();
        }

        private void ValidatePlayButton()
        {
            if (SelectedCharacteristic.HasValue && SelectedDifficulty.HasValue)
            {
                if (_level.AvailableDifficulties.TryGetValue(SelectedCharacteristic.Value, out var difficulties))
                {
                    if (difficulties.Contains(SelectedDifficulty.Value))
                    {
                        _playButton.Enabled = true;
                        return;
                    }
                }
            }

            _playButton.Enabled = false;
        }
    }
}