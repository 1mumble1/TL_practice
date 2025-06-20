import styles from './CommentArea.module.css';

interface CommentAreaProps {
  comment: string;
  onChange: (value: string) => void;
};

const CommentArea: React.FC<CommentAreaProps> = ({ comment, onChange }) => {
    
  return (
    <div className={styles.commentContainer}>
      <textarea
        id="comment"
        placeholder="Напишите, что понравилось, что было непонятно"
        value={comment}
        onChange={(event) => onChange(event.target.value)}
      ></textarea>
    </div>
  );
};

export default CommentArea;