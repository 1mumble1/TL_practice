import type React from 'react';
import AngryFace from '../../../public/twemoji_angry-face.png';
import GrinningFace from '../../../public/twemoji_grinning-face-with-big-eyes.png';
import NeutralFace from '../../../public/twemoji_neutral-face.png';
import FrowningFace from '../../../public/twemoji_slightly-frowning-face.png';
import SmilingFace from '../../../public/twemoji_slightly-smiling-face.png';
import type { Buttons } from '../../types/types';
import styles from './Rating.module.css'
import Button from '../Button/Button';

interface RatingProps {
  pressedButton: number;
  onPressButton: (rating: number) => void;
}

const buttons: Buttons[] = [
  { rate: 1, src: AngryFace, alt: 'AngryFace' },
  { rate: 2, src: FrowningFace, alt: 'FrowningFace' },
  { rate: 3, src: NeutralFace, alt: 'NeutralFace' },
  { rate: 4, src: SmilingFace, alt: 'SmilingFace' },
  { rate: 5, src: GrinningFace, alt: 'GrinningFace' },
];

const Rating: React.FC<RatingProps> = ({ pressedButton, onPressButton }) => {

  const handleOnClick = (rate: number) => {
      const currentRating = rate === pressedButton ? 0 : rate;
      onPressButton(currentRating);
  };

  return (
    <ul className={styles.container}>
      {buttons.map((button) =>
        <li className={styles.item}>
          <Button
            key={button.rate}
            src={button.src}
            alt={button.alt}
            isActive={button.rate === pressedButton}
            onClick={() => handleOnClick(button.rate)}
          />
        </li>
      )}
    </ul>
  );
}

export default Rating;