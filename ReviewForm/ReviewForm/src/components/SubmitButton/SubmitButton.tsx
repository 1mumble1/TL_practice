import type React from 'react';
import styles from './SubmitButton.module.css';

interface SubmitButtonProps {
  isDisabled: boolean;
}

const SubmitButton: React.FC<SubmitButtonProps> = ({ isDisabled }) => {
  return (
    <div className={styles.buttonContainer}>
      <button className={styles.button} type='submit' disabled={isDisabled}>
        Отправить
      </button>
    </div>
  );
};

export default SubmitButton;