import type React from 'react';
import classes from './SubmitButton.module.css';

interface SubmitButtonProps {
  isDisabled: boolean;
}

const SubmitButton: React.FC<SubmitButtonProps> = ({ isDisabled }) => {
  return (
    <button className={classes.button} type='submit' disabled={isDisabled}>
      Отправить
    </button>
  );
}

export default SubmitButton;