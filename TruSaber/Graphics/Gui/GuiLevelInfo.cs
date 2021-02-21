using System;
using System.Linq;
using BeatMapInfo;
using Microsoft.Xna.Framework;
using RocketUI;
using RocketUI.Controls;
using RocketUI.Graphics;
using RocketUI.Layout;
using TruSaber.Models;
using TruSaber.Scenes;

namespace TruSaber.Graphics.Gui
{
    public class GuiLevelInfo : GuiMultiStackContainer
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

        private GuiButtonGroup            _difficultySelect;
        private GuiButtonGroup            _characteristicSelect;
        private GuiTextElement            _title;
        private GuiTextElement            _author;
        private GuiImage                  _albumImage;
        private GuiButton _playButton;

        public Characteristic? SelectedCharacteristic => _characteristicSelect.CheckedControl?.Tag as Characteristic?;
        public Difficulty?     SelectedDifficulty     => _difficultySelect.CheckedControl?.Tag as Difficulty?;
        
        public GuiLevelInfo()
        {
            _difficultySelect = new GuiButtonGroup();
            _title = new GuiTextElement();
            _author = new GuiTextElement();
            _albumImage = new GuiImage(GuiTextures.PanelGlass);
            _difficultySelect = new GuiButtonGroup();
            _difficultySelect.CheckedControlChanged += DifficultySelectOnCheckedControlChanged;
            _characteristicSelect = new GuiButtonGroup();
            _characteristicSelect.CheckedControlChanged += CharacteristicSelectOnCheckedControlChanged;
            _playButton = new GuiButton("PLAY", () => OnPlayClicked());

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
                _characteristicSelect.AddChild(new GuiButton(characteristic.ToString())
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
                    _difficultySelect.AddChild(new GuiButton(difficulty.ToString())
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